using Microsoft.Win32;
using MOLEKULA.MyData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MOLEKULA.Pages
{
    /// <summary>
    /// Логика взаимодействия для Build.xaml
    /// </summary>
    public partial class Build : Page
    {
        public bool mouseDown = false; Point posMouse = new Point(0, 0); int moveAtom=-1;
        private bool drawing = true; int angleConst = 0;
        // Это массив                0             1              2              3              4              5            6                   7            8                  9
        Color[] typeColor = { Colors.Green, Colors.Orange, Colors.Yellow, Colors.Purple, Colors.Silver, Colors.Aqua, Colors.GreenYellow, Colors.Blue, Colors.DarkViolet, Colors.Violet };
        Functionals fun = Functionals.DB();
        bool PlayAnim = false; Random random = new Random();
        public double[] myConst = { -30, -30, 30, 40, 8, 2.4, 1, 0, 0, 0};
        double[] ConstAnimation = { 360, 5 };
        double[] center = { 7, 5, 0 };
        List<int> iterAnim = new List<int>();
        List<Point> pt = new List<Point>(), pt1 = new List<Point>();
        List<string> ptNames = new List<string>();
        List<List<double>> xyz = new List<List<double>>();
        List<double> radiuses = new List<double>();
        List<int> connects = new List<int>();
        List<double> info = new List<double>();
        List<int> queue = new List<int>();
        List<List<string>> molekuls = new List<List<string>>();
        List<string> types = new List<string>();
        private string nameDB = Functionals.DB().nameDB;


        int GetInfoAtom(int id)
        {
            string sql = $"select color from Types, Atoms where " +
                $"Atoms.id_type = Types.id_type and id_atom={id}";

            return fun.GetInt(sql, "color");
        }
        void AddAtomToArr(int id, string type, double x, double y, double z, int anim, int connect = -1)
        {
            if (connect == -1)
                connect = ptNames.Count;
            List<double> ob = new List<double> { x, y, z, id };
            ptNames.Add(type);
            xyz.Add(ob);
            info.Add(GetInfoAtom(id));
            iterAnim.Add(anim);
            connects.Add(connect);

        }
        void LoadFromDataBase(int idMol = 0)
        {
            ClearDate();
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            int id_atom, id_type, anim_i, con, count = 0;
            float x, y, z; string type;

            using (SqlCommand myCommand = new SqlCommand($"select * from Atoms,Mol_atom where id_mol={idMol} and Atoms.id_atom=Mol_atom.id_atom", conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    id_atom = reader.GetInt32(reader.GetOrdinal("id_atom"));
                    id_type = reader.GetInt32(reader.GetOrdinal("id_type"));
                    anim_i = reader.GetInt32(reader.GetOrdinal("anim_i"));
                    x = (float)reader.GetDouble(reader.GetOrdinal("pos_x"));
                    y = (float)reader.GetDouble(reader.GetOrdinal("pos_y"));
                    z = (float)reader.GetDouble(reader.GetOrdinal("pos_z"));
                    type = fun.GetString("Types", "types", "id_type", id_type);
                    con = fun.GetInt("Connects", "id_conn", "id_atom", id_atom);
                    AddAtomToArr(id_atom, type, x, y, z, anim_i, con);
                    radiuses.Add(getRadius(count));
                    queue.Add(count++);
                };
                reader.Close();
            }
            conn.Close();
        }
        void LoadMolekulsFromDB()
        {
            molekuls.Clear();
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            using (SqlCommand myCommand = new SqlCommand($"select * from Molekuls", conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    int id_mol = reader.GetInt32(reader.GetOrdinal("id_mol"));
                    string name_mol = reader.GetString(reader.GetOrdinal("name_mol"));
                    List<string> ob = new List<string> { id_mol.ToString(), name_mol };
                    molekuls.Add(ob);
                };
                reader.Close();
            }

            using (SqlCommand myCommand = new SqlCommand($"select * from Types order by types", conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    string type = reader.GetString(reader.GetOrdinal("types"));
                    types.Add(type);
                };
                reader.Close();
            }

            conn.Close();
        }
        void ClearDate()
        {
            Graf.Children.Clear();
            pt.Clear();
            ptNames.Clear();
            iterAnim.Clear();
            xyz.Clear();
            radiuses.Clear();
            connects.Clear();
            info.Clear();
            queue.Clear();
            types.Clear();
        }

        int FindId(int id)
        {
            for (int i = 0; i < xyz.Count; i++)
                if (id == xyz[i][3])
                    return i;
            return 0;
        }
        void ResetPos()
        {
            double maxiX = 15, maxiY = 10;
            double miniX = -0.4, miniY = -0.3;
            for (int i = 0; i < 0; i++)
            {
                xyz[i][0] = Math.Round(xyz[i][0], 3);
                xyz[i][1] = Math.Round(xyz[i][1], 3);
                xyz[i][2] = Math.Round(xyz[i][2], 3);
                if (xyz[i][1] > maxiY)
                    xyz[i][1] = maxiY;
                if (xyz[i][0] > maxiX)
                    xyz[i][0] = maxiX;

                if (xyz[i][1] < miniY)
                    xyz[i][1] = miniY;
                if (xyz[i][0] < miniX)
                    xyz[i][0] = miniX;
            }
        }
        void Rewrite()
        {
            queue.Clear();
            int selId = Atoms.SelectedIndex, i = 0;
            Atoms.Items.Clear();
            foreach (var ix in ptNames)
            {
                ListBoxItem item = new ListBoxItem
                {
                    Content = ix,
                    Background = Brushes.Transparent,
                    FontSize = 15,
                    Height = 20, Padding = new Thickness(10,0,0,0),
                    Foreground = Brushes.White
                };
                Atoms.Items.Add(item);
                queue.Add(i++);
            }
            DrawMolecula2d();
            Atoms.SelectedIndex = (selId >= Atoms.Items.Count) ? 0 : selId;
        }
        int[,] TransformXY(List<List<double>> xyz1)
        {
            int[,] xy = new int[ptNames.Count, 2];
            for (int i = 0; i < xyz1.Count; i++)
            {
                xy[i, 0] = (int)(xyz1[i][0] * myConst[2]);
                xy[i, 1] = (int)(xyz1[i][1] * myConst[2]);
            }
            return xy;
        }
        void RewriteInfo()
        {
            Molekuls.Items.Clear();
            for (int i = 0; i < molekuls.Count; i++)
            {
                TextBlock bb = new TextBlock();
                bb.Tag = molekuls[i][1];
                foreach (string str in getMolName(molekuls[i][1])) 
                {
                    if (Char.IsDigit(str[0])) bb.Inlines.Add(GetRun(str, true));
                    else bb.Inlines.Add(GetRun(str, false));
                }

                ListBoxItem item = new ListBoxItem
                {
                    Content = bb,
                    Tag = molekuls[i][0],
                    Background = Brushes.Transparent,
                    FontSize = 15,
                    Height = 20, Padding = new Thickness(10, 0, 0, 0),
                    Foreground = Brushes.White
                };
                Molekuls.Items.Add(item);
            }
        }
        List<string> getMolName(string name)
        {
            List<string> list = new List<string>();
            string res = "", num = ""; bool st = true;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if ((Char.IsDigit(c) && st) || (!Char.IsDigit(c) && !st))
                {
                    if (st)
                    {
                        list.Add(res); res = ""; st = !st;
                    }
                    else{
                        list.Add(num); num = ""; st = !st;
                    }
                }

                if (Char.IsDigit(c)) num += c;
                else res += c;
            }
            
            
            if (st && res.Length>0) list.Add(res);
            else if(!st && num.Length > 0) list.Add(num);

            return list;
        }

        int findSmallWay(int id)
        {
            int res = id;
            string name = ptNames[id];
            string findName = "";
            switch (name)
            {
                case "Pt": findName = "null"; break;
                case "Cl": findName = "Pt"; break;
                case "N": findName = "Pt"; break;
                case "H": findName = "N"; break;
            }

            for (int i = 0; i < ptNames.Count; i++)
            {
                double len1 = getLen(new Point(xyz[id][0], xyz[id][1]), new Point(xyz[i][0], xyz[i][1]));
                double len2 = getLen(new Point(xyz[id][0], xyz[id][1]), new Point(xyz[res][0], xyz[res][1]));
                if (ptNames[i] == findName)
                {
                    if (res == id) res = i;
                    if (len2 > len1)
                        res = i;
                }
            }
            return res;
        }
        double getLen(Point dot1, Point dot2)
        {
            return Math.Sqrt(Math.Pow(dot1.X - dot2.X, 2));
        }
        double getRadius(int i)
        {
            return getLen(new Point(center[0], center[1]), new Point(xyz[i][0], xyz[i][1]));
        }
        string ClearText(string str)
        {
            string result = ""; bool flag = false;
            foreach (var k in str)
            {
                if (k == ',')
                {
                    if (!flag)
                    {
                        result += ',';
                        flag = true;
                    }
                }
                if (Char.IsDigit(k))
                    result += k;
            }
            return result;
        }

        void SortQueue()
        {

            for (int i = 0; i < queue.Count; i++)
            {
                for (int j = 0; j < queue.Count; j++)
                {
                    if (xyz[queue[i]][2] < xyz[queue[j]][2])
                    {
                        int mysor = queue[j];
                        queue[j] = queue[i];
                        queue[i] = mysor;
                    }
                }
            }
        }
        void DrawLines(Point ptt1, Point ptt2)
        {
            Line line = new Line
            {
                X1 = ptt1.X,
                Y1 = ptt1.Y,
                X2 = ptt2.X,
                Y2 = ptt2.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };
            Graf.Children.Add(line);
        }
        void DrawConnect(int i)
        {
            int r1 = 30;
            int r2 = 30;
            Point x1 = new Point(pt[i].X + r1, pt[i].Y + r2),
            x2 = new Point(pt[connects[i]].X + r1, pt[connects[i]].Y + r2);
            DrawLines(x1, x2);
        }

        Ellipse DrawBorders(int n1)
        {
            Color[] rgb = { Colors.Red };
            Ellipse border = new Ellipse();
            int r = (int)(myConst[3]) + 2 + (int)((xyz[n1][2] / 2) * myConst[4]);
            r = (r > 1) ? r : 1;
            border.Width = r;
            border.Height = r;
            border.Margin = new Thickness(pt[n1].X - ((r + 1) / 2) - myConst[0], pt[n1].Y - (r / 2) - myConst[1], 0, 0);
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.5));
            radialGradientBrush.GradientStops.Add(new GradientStop(rgb[random.Next(0, rgb.Length)], 1));
            border.Fill = radialGradientBrush;
            return border;
        }
        Label DrawLabel(int n1)
        {
            Color[] rgb = { Colors.Red };
            Label border = new Label();
            int r = (int)(myConst[3]) + 2 + (int)((xyz[n1][2] / 2) * myConst[4]);
            r = (r > 1) ? r : 2;
            border.Width = r;
            border.Height = r;
            border.Margin = new Thickness(pt[n1].X - ((r + 1) / 2) - myConst[0], pt[n1].Y - (r / 2) - myConst[1], 0, 0);
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.5));
            radialGradientBrush.GradientStops.Add(new GradientStop(rgb[random.Next(0, rgb.Length)], 1));

            border.Foreground = Brushes.Black;
            border.Content = ptNames[n1];
            border.VerticalContentAlignment = VerticalAlignment.Center;
            border.HorizontalContentAlignment = HorizontalAlignment.Center;
            border.FontSize = r / 2;
            return border;
        }
        void DrawAtomName(int i)
        {
            Label textL;
            textL = DrawLabel(i);
            textL.MouseUp += new MouseButtonEventHandler(Graf_MouseUp);
            textL.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(Graf_MouseUp));
            Graf.Children.Add(textL);
        }
        void AtomBorder()
        {
            int n1; Ellipse b1;


            if (Atoms.SelectedIndex > -1)
            {
                Graf.Children.Clear();
                DrawMolecula2d();
                n1 = Atoms.SelectedIndex;
                b1 = DrawBorders(n1);
                b1.MouseUp += new MouseButtonEventHandler(Graf_MouseUp);
                b1.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(Graf_MouseUp));
                Graf.Children.Add(b1);
            }
        }
        void DrawMolecula2d()
        {
            if (drawing)
            {
                Graf.Children.Clear();
                ResetPos();
                pt.Clear(); pt1.Clear();
                for (int i = 0; i < xyz.Count; i++)
                {
                    Point newPos = Reset_Angle(angleConst + iterAnim[i], radiuses[i]);
                    xyz[i][0] = newPos.X + center[0];
                    xyz[i][2] = newPos.Y / 120;
                    pt.Add(new Point(TransformXY(xyz)[i, 0], TransformXY(xyz)[i, 1]));
                }
                for (int i = 0; i < xyz.Count; i++) DrawConnect(i);
                SortQueue();
                for (int i = 0; i < queue.Count; i++)
                {
                    int id = queue[i];
                    DrawMol(id);
                    DrawAtomName(id);
                }
            }
        }
        void DrawMol(int i)
        {
            Ellipse ellipse = new Ellipse { Name = "atom" + i };
            int r = (int)(myConst[3]) + (int)((xyz[i][2]/2) * myConst[4]);
            r = (r > 1) ? r : 1;
            ellipse.Width = r;
            ellipse.Height = r;
            ellipse.Margin = new Thickness(pt[i].X - (r / 2) - myConst[0], pt[i].Y - (r / 2) - myConst[1], 0, 0);
            pt1.Add(new Point(ellipse.Margin.Left + (r / 2), ellipse.Margin.Top + (r / 2)));
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
            Color mainColor = typeColor[Convert.ToInt32(info[i])];
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            radialGradientBrush.GradientStops.Add(new GradientStop(mainColor, 1));
            ellipse.Fill = radialGradientBrush;
            ellipse.MouseUp += new MouseButtonEventHandler(Graf_MouseUp);
            ellipse.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(Graf_MouseUp));
            Graf.Children.Add(ellipse);
        }

        private void TBC1(object sender, TextChangedEventArgs e)
        {
            AtomX.Text = ClearText(AtomX.Text);
        }
        private void TBC2(object sender, TextChangedEventArgs e)
        {
            AtomY.Text = ClearText(AtomY.Text);
        }
        private void TBC3(object sender, TextChangedEventArgs e)
        {
            AtomZ.Text = ClearText(AtomZ.Text);
        }
        private void Atom_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidSetPos();
        }
        private void Molekula_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock tt = new TextBlock();
            string res = Molekula_Name.Text;
            if (res.Length > 8)
                res = res.Substring(0, 8);
            Molekula_Name.Text = res;
            Show_Name.Text = "";
        }
        private void AddAtom_Click(object sender, RoutedEventArgs e)
        {
            if (Molekuls.SelectedIndex > -1)
            {
                ValidSetPos(); drawing = false;
                string xName = AtomName.Text;
                if (xName.Length == 0) xName = "N";
                ptNames.Add(xName);
                List<double> ob = new List<double>
            {
                Convert.ToDouble(AtomX.Text),
                Convert.ToDouble(AtomY.Text),
                Convert.ToDouble(AtomZ.Text),
                0
            };
                xyz.Add(ob);
                iterAnim.Add(((ob[0] > center[0]) ? 90 : 270) + random.Next(-20, 20));
                radiuses.Add(getRadius(xyz.Count - 1));
                connects.Add(queue.Count); //SetConnect(xyz.Count - 1);
                queue.Add(xyz.Count - 1);
                info.Add(fun.GetColor(xName)); drawing = true;
                Rewrite();
            }
        }
        private void CreateConnect(object sender, RoutedEventArgs e)
        {
            if (Atoms.SelectedIndex > -1)
            {
                myConst[7] = 1;
                MessageBox.Show("Выберите необходимый атом к которому нужно провести связь!");
            }

        }
        private void DeleteConnect(object sender, RoutedEventArgs e)
        {
            int n1 = Atoms.SelectedIndex;
            if (n1 > -1 && n1 < ptNames.Count)
                connects[n1] = n1;
            DrawMolecula2d();

        }
        private void DellAtom_Click(object sender, RoutedEventArgs e)
        {
            int n1 = Atoms.SelectedIndex;
            if (n1 > -1)
            {
                ptNames.RemoveAt(n1);
                xyz.RemoveAt(n1);
                iterAnim.RemoveAt(n1);
                radiuses.RemoveAt(n1);
                info.RemoveAt(n1);
                AtomName.SelectedIndex = -1;
                Rewrite(); DrawMolecula2d();
            }

        }
        private void SaveAtoms(int saveMol = -1)
        {
            int n1 = Molekuls.SelectedIndex;
            for (int i = 0; i < ptNames.Count; i++)
            {
                string t;
                int id_t;
                string getCount = $"select * from Atoms where id_atom = {xyz[i][3]}";
                t = ptNames[i]; id_t = fun.GetInt("Types", "id_type", "types", t);
                int count = fun.GetCount(getCount);
                if (count == 0)
                    fun.Add(id_t, xyz[i][0], xyz[i][1], xyz[i][2], iterAnim[i], connects[i]);
                else
                    fun.Update((int)xyz[i][3], id_t, xyz[i][0], xyz[i][1], xyz[i][2], iterAnim[i], connects[i]);


                if (saveMol >= 0 && n1 > -1)
                {
                    int id_atom = (count == 0) ? fun.GetId() : (int)xyz[i][3];
                    fun.AddMolAtom(saveMol, id_atom);
                }
            }
        }
        private Run GetRun(string text, bool index)
        {
            Run run = new Run(text);
            if (index)
            {
                run.BaselineAlignment = BaselineAlignment.Subscript;
                run.FontSize = 10;
            }
            return run;
        }

        private void Build_Molekul(object sender, RoutedEventArgs e)
        {
            Graf.Children.Clear();
            DrawMolecula2d();
        }
        private void Edited_Atom(object sender, RoutedEventArgs e)
        {
            ValidSetPos();
            int n1 = Atoms.SelectedIndex;
            n1 = (n1 >= 0) ? n1 : 0;

            double x = Convert.ToDouble(AtomX.Text);
            double y = Convert.ToDouble(AtomY.Text);
            double z = Convert.ToDouble(AtomZ.Text);
            ptNames[n1] = (AtomName.Text.Length > 0) ? AtomName.Text : ptNames[n1];
            xyz[n1][0] = x;
            xyz[n1][1] = y;
            xyz[n1][2] = z;
            radiuses[n1] = getRadius(n1);
            info[n1] = fun.GetColor(ptNames[n1]);
            Graf.Children.Clear();
            Atoms.Items.Clear();
            for (int i = 0; i < ptNames.Count; i++)
            {
                ListBoxItem obb = new ListBoxItem
                {
                    Content = ptNames[i],
                    Background = Brushes.Transparent
                };
                Atoms.Items.Add(obb);
            }
            Atoms.SelectedIndex = n1;
            while (x < center[0] && (iterAnim[n1] > 0 && iterAnim[n1] < 180))
                iterAnim[n1] += 90;
            while (x > center[0] && (iterAnim[n1] < 0 && iterAnim[n1] > 180))
                iterAnim[n1] -= 90;

            DrawMolecula2d();
            AtomBorder();
        }
        private void SelectAtom(object sender, SelectionChangedEventArgs e)
        {
            AtomBorder();
            if (Atoms.SelectedIndex > -1)
            {
                int n1 = Atoms.SelectedIndex;
                int iddd = -1; string name = "H"; if (Atoms.SelectedItem != null)
                    name = Atoms.SelectedItem.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");
                AtomName.SelectedIndex = iddd;

                

                AtomX.Text = (Math.Round(xyz[n1][0] * 1000) / 1000).ToString();
                AtomY.Text = (Math.Round(xyz[n1][1] * 1000) / 1000).ToString();
                AtomZ.Text = (Math.Round(xyz[n1][2] * 1000) / 1000).ToString();
            }
        }
        private void SetAtomIters(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int n1 = Atoms.SelectedIndex;

            if (n1 > -1 && n1 < ptNames.Count)
            {
                iterAnim[n1] = (int)SetAtomIter.Value;
                int angle = (int)LoadAnimation.Value;
                RotateAtoms(angle);
                DrawMolecula2d();
            }
        }
        private void ValidSetPos()
        {
            try
            {
                string x = AtomX.Text, y = AtomY.Text, z = AtomZ.Text;

                if (x.Length == 0) x = $"{center[0]}";
                if (y.Length == 0) y = $"{center[1]}";
                if (z.Length == 0) z = $"{center[2]}";

                foreach (char i in x)
                    if (!Char.IsDigit(i) && i != ',')
                        x = $"{center[0]}";

                foreach (char i in y)
                    if (!Char.IsDigit(i) && i != ',')
                        y = $"{center[0]}";

                foreach (char i in z)
                    if (!Char.IsDigit(i) && i != ',')
                        z = $"{center[0]}";

                double xx = Convert.ToDouble(x), yy = Convert.ToDouble(y), zz = Convert.ToDouble(z);
                if (xx < 1 || xx > 15) x = $"{center[0]}";
                if (yy < 1 || yy > 10) y = $"{center[1]}";
                if (zz < -50 || zz > 50) z = $"{center[2]}";

                AtomX.Text = x;
                AtomY.Text = y;
                AtomZ.Text = z;
            }
            catch
            {
                AtomX.Text = "7";
                AtomY.Text = "5";
                AtomZ.Text = "0";
            }
            
        }
        private void SetPosAtom(double x, double y, double z, int id)
        {
            int n1 = id;
            n1 = (n1 >= 0) ? n1 : 0;

            xyz[n1][0] = x;
            xyz[n1][1] = y;
            xyz[n1][2] = z;

            radiuses[n1] = getRadius(n1);
            Graf.Children.Clear();
            DrawMolecula2d();
            AtomBorder();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            PlayAnim = !PlayAnim; int val = (int)LoadAnimation.Value;
            if (PlayAnim)
            {
                EditMaket.IsEnabled = false;
                DoubleAnimation anim = new DoubleAnimation
                {
                    From = val,
                    To = ConstAnimation[0],
                    Duration = TimeSpan.FromSeconds(ConstAnimation[1])
                };
                LoadAnimation.BeginAnimation(ProgressBar.ValueProperty, anim);
            }
            else { 
                if(myConst[8]==1) EditMaket.IsEnabled = true;
                LoadAnimation.BeginAnimation(ProgressBar.ValueProperty, null); LoadAnimation.Value = val; 
            };
        }
        private void PlayAnimation(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LoadAnimation.Value == ConstAnimation[0])
            {
                if (PlayAnim)
                {
                    DoubleAnimation anim = new DoubleAnimation
                    {
                        From = 0,
                        To = ConstAnimation[0],
                        Duration = TimeSpan.FromSeconds(ConstAnimation[1])
                    };
                    LoadAnimation.BeginAnimation(ProgressBar.ValueProperty, anim);
                }
                LoadAnimation.Value = 0;
            }

            angleConst = (int)LoadAnimation.Value;
            DrawMolecula2d();
            AtomBorder();
        }
        private void RotateAtoms(int angle)
        {
            for (int i = 0; i < xyz.Count; i++)
            {
                Point newPos = Reset_Angle(angle + iterAnim[i], radiuses[i]);
                xyz[i][0] = newPos.X + center[0];
                xyz[i][2] = newPos.Y / 120;
            }
        }
        private void RotateX(int iter = -1)
        {
            int iteration = (iter == -1) ? (int)LoadAnimation.Value : iter;
            for (int i = 0; i < xyz.Count; i++)
            {
                double angle = 0.0174532925 * (iteration + iterAnim[i]);
                double res, radius = radiuses[i] * 100;
                res = (int)(radius * Math.Sin(angle)); res /= 100;
                xyz[i][0] = res + center[0];
                res = (int)(radius * Math.Cos(angle));
                xyz[i][2] = res / 105;
            }
            DrawMolecula2d();
        }
        private void RotateY(int iter = -1)
        {
            int iteration = (iter == -1) ? (int)LoadAnimation.Value : iter;
            for (int i = 0; i < xyz.Count; i++)
            {
                double angle = 0.0174532925 * (iteration + iterAnim[i]);
                double res, radius = radiuses[i] * 100;
                res = (int)(radius * Math.Sin(angle)); res /= 100;
                xyz[i][1] = res + center[1];
                res = (int)(radius * Math.Cos(angle));
                xyz[i][2] = res / 105;
            }
            DrawMolecula2d();
        }
        private Point Reset_Angle(int iteration, double radius)
        {
            Point res = new Point(); radius *= 100;
            double angle = 0.0174532925 * iteration;
            res.X = (int)(radius * Math.Sin(angle)); res.X /= 100;
            res.Y = (int)(radius * Math.Cos(angle));
            return res;
        }

        
        private void PrevMol(object sender, RoutedEventArgs e)
        {
            myConst[6]--; if (fun.GetCount($"select * from Molekuls where id_mol = {myConst[6]}") == 0)
                myConst[6] = 1;
            LoadFromDataBase((int)myConst[6]);
            Rewrite(); RewriteInfo();
        }
        private void NextMol(object sender, RoutedEventArgs e)
        {
            myConst[6]++; if (fun.GetCount($"select * from Molekuls where id_mol = {myConst[6]}") == 0)
                myConst[6] = 1;
            LoadFromDataBase((int)myConst[6]);
            Rewrite(); RewriteInfo();
        }
        private void Save_Molekula(int idx)
        {
            List<string> mol = molekuls[idx];
            int id = Convert.ToInt32(mol[0]);
            string name = mol[1];
            fun.AddMolekula(id, name);
        }
        private void Clear_Molekula(object sender, RoutedEventArgs e)
        {
            if (Molekuls.SelectedIndex > -1)
            {
                ClearDate(); Rewrite();
            }
            else MessageBox.Show("Выберите молекулу!");
        }
        private void Save_Molekula(object sender, RoutedEventArgs e)
        {
            try
            {
                int n1 = Molekuls.SelectedIndex;
                if (n1 >= 0)
                {
                    string names = Molekula_Name.Text;
                    if (names == "" && Show_Name.Text != "")
                        names = Show_Name.Text;
                    molekuls[n1][1] = names;
                    int id = Convert.ToInt32(molekuls[n1][0]);
                    fun.DeleteFrom(id, "Mol_atom", "id_mol");
                    fun.Delete($"Delete from Atoms where id_atom NOT IN (Select id_atom from Mol_atom)");
                    Save_Molekula(n1);
                    SaveAtoms(id);

                    RewriteInfo();
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка при работе с Базой данных!");
            }
        }
        private void Dell_Molekula(object sender, RoutedEventArgs e)
        {
            try
            {
                int n1 = Molekuls.SelectedIndex;
                if (n1 >= 0)
                {
                    int id = Convert.ToInt32(molekuls[n1][0]);
                    molekuls.RemoveAt(n1);
                    RewriteInfo(); Atoms.Items.Clear();
                    if (molekuls.Count == 0)
                        Graf.Children.Clear();
                    fun.DeleteFrom(id, "Mol_atom", "id_mol");
                    fun.Delete($"Delete from Atoms where id_atom NOT IN (Select id_atom from Mol_atom)");
                    fun.DeleteFrom(id, "Molekuls", "id_mol");
                    Molekula_Name.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка при работе с Базой данных!");
            }
            
        }
        private void Add_Molekula(object sender, RoutedEventArgs e)
        {
            int new_id = fun.GetId("Molekuls", "id_mol");
            molekuls.Add(new List<string>() { $"{++new_id}", "NAME" });
            RewriteInfo(); ClearDate();
        }
        private void SelectMolekuls(object sender, SelectionChangedEventArgs e)
        {
            if (Molekuls.Items.Count > 0)
            {
                Show_Name.Text = "";
                ListBoxItem item = (ListBoxItem)Molekuls.SelectedItem;
                TextBlock text = (TextBlock)(item.Content);
                Molekula_Name.Text = "";
                foreach (string str in getMolName(text.Tag.ToString()))
                {
                    if (Char.IsDigit(str[0])) Show_Name.Inlines.Add(GetRun(str, true));
                    else Show_Name.Inlines.Add(GetRun(str, false));
                }
                myConst[6] = Convert.ToDouble(item.Tag.ToString());
                LoadFromDataBase((int)myConst[6]);
                Rewrite();
            }
            Atoms.SelectedIndex = -1;
        }

        private void Resize(object sender, RoutedEventArgs e)
        {
            if (myConst[8] == 0)
            {
                if (!PlayAnim) 
                    EditMaket.IsEnabled = true;
                myConst[8] = 1;
                bord1.Width = 870;
                bord1.Height = 470;
                ResizeButton.Margin = new Thickness(34, 0, 0, 425);
                RotateButt.Margin = new Thickness(834, 0, 0, 20);
                ClearButt.Margin = new Thickness(789, 0, 0, 20);
                SaveImg.Margin = new Thickness(744, 0, 0, 20);
                EditMaket.Margin = new Thickness(699, 0, 0, 20);
                bord1.Margin = new Thickness(20, 0, 0, 0);
            }
            else
            {
                EditMaket.IsEnabled = false;
                ResizeButton.Margin = new Thickness(34, 0, 0, 410);
                RotateButt.Margin = new Thickness(484, 0, 0, 47);
                ClearButt.Margin = new Thickness(439, 0, 0, 47);
                SaveImg.Margin = new Thickness(394, 0, 0, 47);
                EditMaket.Margin = new Thickness(349, 0, 0, 47);
                myConst[8] = 0;
                bord1.Width = 523;
                bord1.Height = 426;
                bord1.Margin = new Thickness(20, 15, 0, 0);
            }
        }

        private void Clear_Canvas(object sender, RoutedEventArgs e)
        {
            PlayAnim = false;
            LoadAnimation.BeginAnimation(ProgressBar.ValueProperty, null);
            LoadAnimation.Value = 0;
            Graf.Children.Clear();
        }
        private void EditCanvas(object sender, RoutedEventArgs e)
        {
            if (myConst[9] == 0) myConst[9] = 1;
            else myConst[9] = 0;
            
        }

        private void EditFalse(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(!EditMaket.IsEnabled)
                myConst[9] = 0;
        }

        private void Graf_Mouse_Down(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseDown = true;
            Point p = Mouse.GetPosition(Graf); int idx = -1;
            for (int i = 0; i < xyz.Count; i++)
            {
                if (p.X > xyz[i][0] * myConst[2] && p.X < xyz[i][0] * myConst[2] + myConst[3] + 20)
                {
                    if (p.Y > xyz[i][1] * myConst[2] && p.Y < xyz[i][1] * myConst[2] + myConst[3] + 20)
                    {
                        if (idx != -1) { if (xyz[idx][2] < xyz[i][2]) idx = i; }
                        else idx = i;
                    }
                }
            }
            if (idx != -1)
            {
                posMouse = p; moveAtom = idx;
            }
            
            if (myConst[7] == 0) { Atoms.SelectedIndex = idx; }
            else
            {
                myConst[7] = 0;
                if (Atoms.SelectedIndex >= 0)
                {
                    connects[Atoms.SelectedIndex] = (idx >= 0) ? idx : connects[Atoms.SelectedIndex];
                    Atoms.SelectedIndex = -1;
                    MessageBox.Show("Успешно");
                }
                Create_Connect.BorderBrush = new SolidColorBrush(Colors.Blue);
                DrawMolecula2d();
            }
        }
        private void Graf_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(Graf);
            for (int i = 0; i < xyz.Count; i++)
            {
                if (p.X > xyz[i][0] * myConst[2] && p.X < xyz[i][0] * myConst[2] + myConst[3] + 20)
                {
                    if (p.Y > xyz[i][1] * myConst[2] && p.Y < xyz[i][1] * myConst[2] + myConst[3] + 20)
                    { Cursor = Cursors.Hand; break; }
                    else Cursor = Cursors.Arrow;
                }else Cursor = Cursors.Arrow;
            }
            Point pp = new Point(-1, -1);
            if (mouseDown && posMouse != pp && myConst[9]==1 && moveAtom!=-1) 
            {
                xyz[moveAtom][0] = (p.X - 20) / myConst[2];
                xyz[moveAtom][1] = (p.Y - 20) / myConst[2];
                if (xyz[moveAtom][0] < center[0] && iterAnim[moveAtom] < 180) iterAnim[moveAtom] += 180;
                if (xyz[moveAtom][0] > center[0] && iterAnim[moveAtom] > 180) iterAnim[moveAtom] -= 180;
                radiuses[moveAtom] = getRadius(moveAtom);
                DrawMolecula2d();
            }

        }

        private void Graf_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            posMouse = new Point(-1, -1); moveAtom = -1;
        }

        private void EditMaket_Checked(object sender, RoutedEventArgs e)
        {
            mouseDown = false;
        }

        private void SaveToImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = ".png|*.png"; if (Molekuls.Items.Count > 0)
            {
                ListBoxItem item = (ListBoxItem)Molekuls.SelectedItem;
                TextBlock text = (TextBlock)(item.Content);
                s.FileName = text.Tag.ToString() + ".png";
            }
            string path = Directory.GetCurrentDirectory() + $"\\molekul.png";
            if ((bool)s.ShowDialog()) path = s.FileName;
            try
            {
                ExportToPng(Graf, path, Graf.RenderSize);
            }
            catch { };

        }

        public void ExportToPng(Canvas canvas, string path, Size size)
        {
            
            if (path == null) return;
            canvas.Measure(size);
            var rect = new Rect(size);
            canvas.Arrange(rect);
            Render(canvas, size, path);
        }


        private void Render(Visual visual, Size size, string path)
        {
            double dpi = 128;
            double scale = dpi / 96;
            RenderTargetBitmap renderBitmap =
                new RenderTargetBitmap(
                (int)(size.Width * scale),
                (int)(size.Height * scale),
                dpi,
                dpi,
                PixelFormats.Pbgra32);
            renderBitmap.Render(visual);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
        }



        public Build()
        {
            InitializeComponent();
            LoadMolekulsFromDB();
            Rewrite();
            RewriteInfo();
            AtomName.Items.Clear();
            foreach (string i in types)
                AtomName.Items.Add(i);
        }
    }
}

using System.Diagnostics;

namespace Wordle
{
    public partial class Form1 : Form
    {
        Dictionary<int, BlockInfomation> Blocks = new();
        string[] ARTheme = new string[5];
        string Theme;
        Bitmap canvas;
        Graphics g;
        int NowBlock = 0;
        public Form1()
        {
            InitializeComponent();
            canvas = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(canvas);

            Theme = GetTheme();
            for (int i = 0; i < Theme.Length; i++)
            {
                ARTheme[i] = Theme[i].ToString();
            }
            DrawLabelBox();
        }

        /// <summary>
        /// WordList���烉���_���Ɍ��t��I�т܂�
        /// </summary>
        /// <returns>word</returns>
        private string GetTheme()
        {
            Random ramdom = new();
            int ramdomint = ramdom.Next(0, 13);
            WordList.Word word = (WordList.Word)Enum.ToObject(typeof(WordList.Word), ramdomint);
            Debug.WriteLine("Theme: " + word);
            return word.ToString();
        }

        private void DrawLabelBox()
        {
            Font fnt = new("MS UI Gothic", 30);
            Pen p = new(Color.Black, 2);
            //����̈�ԏ��߂̐}�`�ł��B��������ɂق��̐}�`��`�悵�Ă����܂�
            Rectangle reference = new(50, 50, 40, 40);

            int count = 0;
            for (int c = 0; c < 6; c++)
            {
                for (int i = 0; i < 5; i++)
                {
                    //�`��A�}�`�̊Ԋu��ݒ�ł��܂�
                    Rectangle r = new(reference.X * i + 150, reference.Y * c + 50, reference.Width, reference.Height);
                    BlockInfomation fi = new()
                    {
                        X = reference.X * i + 150,
                        Y = reference.Y * c + 50,
                        SideBlock = GetSideBlock(count)
                    };
                    Blocks.Add(count, fi);
                    g.FillRectangle(Brushes.Gray, r);
                    count++;
                }
            }
            //���̕`��
            g.DrawString("��", fnt, Brushes.White, Blocks[0].X - 50, Blocks[0].Y);

            pictureBox1.Image = canvas;
            p.Dispose();
        }

        int keycount = 0;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Font fnt = new("MS UI Gothic", 30);

            //BackSpace�̎�
            if ((e.KeyCode == Keys.Back) && (keycount != 0) && (NowBlock * 5 < keycount))
            {
                keycount--;
                Rectangle reference = new(Blocks[keycount].X, Blocks[keycount].Y, 40, 40);
                g.FillRectangle(Brushes.Gray, reference);
                pictureBox1.Image = canvas;
                return;
            }

            //Enter�̎�
            if (e.KeyCode == Keys.Enter && (Blocks[NowBlock * 5].SideBlock * 5) + 5 == keycount)
            {
                //�����������ɂ��邩
                for (int i = 0; i < 5; i++)
                {
                    if (Theme.Contains(Blocks[(NowBlock * 5) + i].Text))
                    {
                        //���F�ɂ���
                        Rectangle greenreference = new(Blocks[(NowBlock * 5) + i].X, Blocks[(NowBlock * 5) + i].Y, 40, 40);
                        g.FillRectangle(Brushes.Orange, greenreference);
                        g.DrawString(Blocks[(NowBlock * 5) + i].Text, fnt, Brushes.White, Blocks[(NowBlock * 5) + i].X, Blocks[(NowBlock * 5) + i].Y);
                        pictureBox1.Image = canvas;

                    }
                }

                //�ʒu�������Ă邩
                for (int i = 0; i < 5; i++)
                {
                    string anser = Blocks[(NowBlock * 5) + i].Text; // ����
                    if (ARTheme[i].Equals(anser))
                    {
                        //�΂ɂ���
                        Rectangle greenreference = new(Blocks[(NowBlock * 5) + i].X, Blocks[(NowBlock * 5) + i].Y, 40, 40);
                        g.FillRectangle(Brushes.Green, greenreference);
                        g.DrawString(Blocks[(NowBlock * 5) + i].Text, fnt, Brushes.White, Blocks[(NowBlock * 5) + i].X, Blocks[(NowBlock * 5) + i].Y);
                        pictureBox1.Image = canvas;
                        
                    }
                }

                NowBlock++;
                Debug.WriteLine("Enter -- NowBlock++");
                if (NowBlock == 6)
                {
                    Debug.WriteLine("End");
                    Ending();
                    return;
                }

                //�������ĉ��s����
                Rectangle reference = new(Blocks[0].X - 50, Blocks[keycount - 4].Y, 47, 40);
                g.FillRectangle(Brushes.Black, reference);
                g.DrawString("��", fnt, Brushes.White, Blocks[0].X - 50, Blocks[keycount].Y);
                pictureBox1.Image = canvas;

                return;
            }

            //1�����ȏ�̏ꍇ�͂Ȃ����Ƃɂ���
            if (e.KeyCode.ToString().Length > 1) return;
            //���E�𒴂����Ⴞ�߂���
            if (keycount >= 30) return;
            //�s���щz���ē��͂ł��Ȃ��悤�ɂ���
            if ((Blocks[NowBlock * 5].SideBlock * 5) + 4 < keycount) return;

            Blocks[keycount].Text = e.KeyCode.ToString();
            g.DrawString(e.KeyCode.ToString(), fnt, Brushes.White, Blocks[keycount].X, Blocks[keycount].Y);
            fnt.Dispose();
            pictureBox1.Image = canvas;
            keycount++;
        }

        public void Ending()
        {

        }

        public int GetSideBlock(int count)
        {
            if (count < 4 + 1) return 0;
            if (count > 4 && count < 10) return 1;
            if (count > 9 && count < 15) return 2;
            if (count > 14 && count < 20) return 3;
            if (count > 19 && count < 25) return 4;
            if (count > 24 && count < 30) return 5;
            return 0;
        }
    }

    public class BlockInfomation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string? Text { get; set; }
        public int SideBlock { get; set; }
    }
}
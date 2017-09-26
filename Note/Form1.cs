using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Note
{
    public partial class Maiform : Form
    {
        private string filename = "";//用来将文件名保存以显示
        private bool issaved = false;//检查是否显示
        public Maiform()
        {
            InitializeComponent();
            xinjian.Click += new_menu_item_Click;
            open.Click += Open_Menu_item_Click;
            save.Click += Save_Menu_item_Click; 
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maiform_Load(object sender, EventArgs e)
        {
            this.Text = "_无标题";
        }
        /// <summary>
        /// 菜单新建选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void new_menu_item_Click(object sender, EventArgs e)
        {       
            if (issaved==false)//文件未保存,空写textbox
            {
                DialogResult dr = MessageBox.Show(null, "文件尚未保存，是否保存？", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr==DialogResult.Cancel)
                {
                    return;
                }
                if(dr==DialogResult.Yes)
                {
                    WriteToText();
                }
               
            }
            //新建空文本
            
            this.Text = "_无标题";
            richTextBox1.Clear();
            this.issaved = false;//清除标志位
            
        }
        /// <summary>
        /// 菜单打开选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Menu_item_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt";//文件筛选器
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;//保存文件名
                using (StreamReader sr = new StreamReader(filename, Encoding.Default))
                {
                    richTextBox1.Text = sr.ReadToEnd();//文件流方式读取内容并显示
                    this.issaved = true;
                    CharCount.Text = richTextBox1.TextLength.ToString();//显示文档中的字符数
                }
                this.Text = string.Format("{0}",Path.GetFileName(filename));//标题栏显式文件名
            }
        }
        /// <summary>
        /// 菜单保存选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Menu_item_Click(object sender, EventArgs e)
        {
            WriteToText();
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        private void WriteToText()
        {
            if (filename == "")//未保存的新文件
            {
                saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    using (StreamWriter sw = new StreamWriter(filename, false, Encoding.Default)) //不可追加的方式写入
                    {
                        sw.WriteLine(richTextBox1.Text);//把内容写入流对象
                        sw.Flush();//把流中的数据刷新到文件
                        this.issaved = true;
                        this.Text = string.Format("{0}", Path.GetFileName(filename));//标题栏显式文件名
                        MessageBox.Show("保存成功！");
                    }

                }
                
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(filename, false, Encoding.Default))
                {
                    sw.WriteLine(richTextBox1.Text);
                    sw.Flush();
                    this.issaved = true;
                    this.Text = string.Format("{0}", Path.GetFileName(filename));//标题栏显式文件名
                    MessageBox.Show("保存成功！");
                }
            }
        }
        /// <summary>
        /// 文本发生变化时清除标志位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.issaved = false;
            CharCount.Text= richTextBox1.TextLength.ToString();//显示文档中的字符数
        }
        /// <summary>
        /// 菜单另存为选项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAs_Menu_Item_Click(object sender, EventArgs e)
        {   
            //这样设置可以完全当作保存新文件来操作
            filename = "";
            WriteToText();
        }
        /// <summary>
        /// 菜单栏剪切选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cut_Menu_Item_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }
        /// <summary>
        /// 菜单栏复制选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Copy_Menu_Item_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }
        /// <summary>
        /// 菜单栏粘贴选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Paste_Menu_Item_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }
        /// <summary>
        /// 菜单栏全选选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void All_Menu_Item_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }
        /// <summary>
        /// 菜单自动换行选项的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void WordWrap_Menu_Item_Click(object sender, EventArgs e)
        {
            richTextBox1.WordWrap = WordWrap_Menu_Item.Checked;
        }
        /// <summary>
        /// 菜单字体选项的注册事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Font_Menu_Item_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.Font;//默认字体和颜色
            fontDialog1.Color = richTextBox1.ForeColor;
            if (fontDialog1.ShowDialog()==DialogResult.OK)
            {
                //设置文本的字体和颜色，但是此格式并不会保存下来。需要设置一个xml配置文件
                richTextBox1.Font = fontDialog1.Font;
                richTextBox1.ForeColor = fontDialog1.Color;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlLibrary
{
    public partial class IPAddressBox : UserControl
    {
        public IPAddressBox()
        {
            InitializeComponent();
        }
        [Category("属性已更改")]
        [Browsable(true)]
        public new event EventHandler TextChanged;

        private string _text;
        [Category("外观")]
        [Description("与空间关联的文本")]
        [Browsable(true)]
        public new string Text
        {
            get
            {
                if (this.textBox1.Text.Length == 0
                    || this.textBox2.Text.Length == 0
                    || this.textBox3.Text.Length == 0
                    || this.textBox4.Text.Length == 0)
                {
                    _text = "";
                    return _text;
                }
                else
                {
                    _text = Convert.ToInt32(this.textBox1.Text).ToString() + "." +
                            Convert.ToInt32(this.textBox2.Text).ToString() + "." +
                            Convert.ToInt32(this.textBox3.Text).ToString() + "." +
                            Convert.ToInt32(this.textBox4.Text).ToString();
                    return _text;
                }
            }
            set
            {
                if (value != null)
                {
                    string[] strs = value.Split('.');
                    Int32[] num = new Int32[4];
                    if (strs.Length == 4)
                    {
                        bool result = true;
                        for (int i = 0; i < 4; i++)
                        {
                            result = result && Int32.TryParse(strs[i], out num[i]);
                        }
                        if (result && num[0] <= 223 && num[1] <= 255
                            && num[2] <= 255 && num[3] <= 255)
                        {
                            this.textBox1.Text = strs[0];
                            this.textBox2.Text = strs[1];
                            this.textBox3.Text = strs[2];
                            this.textBox4.Text = strs[3];
                        }
                    }
                    else
                    {
                        this.textBox1.Text = "";
                        this.textBox2.Text = "";
                        this.textBox3.Text = "";
                        this.textBox4.Text = "";
                        _text = "";
                    }
                }
                _text = value;
            }
        }
        private void MaskIPAddr(TextBox textBox, KeyPressEventArgs e)
        {
            //判断输入的值是否为数字或删除键
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                if (e.KeyChar != 8 && textBox.Text.Length == 2)
                {
                    string tempStr = textBox.Text + e.KeyChar;
                    if (textBox.Name == "textBox1")
                    {
                        if (Int32.Parse(tempStr) > 223)
                        {
                            MessageBox.Show(tempStr + " 不是一个有效项目。请指定一个介于 1 和 223 之间的数值。");
                            textBox.Text = "223";
                            textBox.Focus();
                            return;
                        }
                        this.textBox2.Focus();
                        this.textBox2.SelectAll();
                    }
                    else if (textBox.Name == "textBox2")
                    {
                        if (Int32.Parse(tempStr) > 255)
                        {
                            MessageBox.Show(tempStr + "不是一个有效项目。请指定一个介于 1 和 255 之间的数值.");
                            textBox.Text = "255";
                            textBox.Focus();
                            return;
                        }
                        this.textBox3.Focus();
                        this.textBox3.SelectAll();
                    }
                    else if (textBox.Name == "textBox3")
                    {
                        if (Int32.Parse(tempStr) > 255)
                        {
                            MessageBox.Show(tempStr + "不是一个有效项目。请指定一个介于 1 和 223 之间的数值。");
                            textBox.Text = "255";
                            textBox.Focus();
                            return;
                        }
                        this.textBox4.Focus();
                        this.textBox4.SelectAll();
                    }
                    else if (textBox.Name == "textBox4")
                    {
                        if (Int32.Parse(tempStr) > 255)
                        {
                            MessageBox.Show(tempStr + "不是一个有效项目。请指定一个介于 1 和 223 之间的数值。");
                            textBox.Text = "255";
                            textBox.Focus();
                            return;
                        }
                    }
                }
                else if (e.KeyChar == 8)
                {
                    if (textBox.Name == "textBox4" && textBox.Text.Length == 0)
                    {
                        this.textBox3.Focus();
                    }
                    else if (textBox.Name == "textBox3" && textBox.Text.Length == 0)
                    {
                        this.textBox2.Focus();
                    }
                    else if (textBox.Name == "textBox2" && textBox.Text.Length == 0)
                    {
                        this.textBox1.Focus();
                    }
                }
            }
            else
            {
                e.Handled = true;
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            MaskIPAddr(this.textBox1, e);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            MaskIPAddr(this.textBox2, e);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            MaskIPAddr(this.textBox3, e);
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            MaskIPAddr(this.textBox4, e);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length != 0 && Int32.Parse(this.textBox1.Text) > 223)
            {
                MessageBox.Show(this.textBox1.Text + "不是一个有效项目,请指定一个介于 1 和 223 之间的数值。");
                this.textBox1.Text = "223";
                this.textBox1.Focus();
            }
            OnResize(this, new EventArgs());
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox2.Text.Length != 0 && Int32.Parse(this.textBox2.Text) > 255)
            {
                MessageBox.Show(this.textBox2.Text + "不是一个有效项目。请指定一个介于 1 和 255 之间的数值。");
                this.textBox2.Text = "255";
                this.textBox2.Focus();
            }
            OnResize(this, new EventArgs());
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox3.Text.Length != 0 && Int32.Parse(this.textBox3.Text) > 255)
            {
                MessageBox.Show(this.textBox3.Text + "不是一个有效项目。请指定一个介于 1 和 255 之间的数值。");
                this.textBox3.Text = "255";
                this.textBox3.Focus();
            }
            OnResize(this, new EventArgs());
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox4.Text.Length != 0 && Int32.Parse(this.textBox4.Text) > 255)
            {
                MessageBox.Show(this.textBox4.Text + "不是一个有效项目。请指定一个介于 1 和 255 之间的数值。");
                this.textBox4.Text = "255";
                this.textBox4.Focus();
            }
            OnResize(this, new EventArgs());
        }
        protected void OnResize(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }
    }
}

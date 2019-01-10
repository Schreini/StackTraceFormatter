using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrackTraceFormatter.Services;

namespace StackTraceFormatter
{
    public partial class Form1 : Form
    {
        private FormatterService _formatter;

        public Form1()
        {
            InitializeComponent();
            _formatter = new FormatterService();
        }

        private void memoEdit1_TextChanged(object sender, EventArgs e)
        {
            var formatted = _formatter.Format(TxtInput.Text);
            TxtOutput.Text = formatted;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvoidDeadlocks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Will deadlock?", "Potential Deadlock");

            var task = DownloadAsync("https://leandrosilva.com.br", false);
            var content = task.Result.Length.ToString(); // no, will not deadlock here

            MessageBox.Show($"Not deadlocked, boy! ({content.Length} bytes)", "No Deadlock");

            MessageBox.Show("How about now?", "Potential Deadlock");

            task = DownloadAsync("https://leandrosilva.com.br", true);
            content = task.Result.Length.ToString(); // potential deadlock here

            MessageBox.Show($"Didn't got deadlocked! ({content.Length} bytes)", "Potential Deadlock");
        }

        async Task<string> DownloadAsync(string url, bool continueOnCapturedContext)
        {
            using (var httpClient = new HttpClient())
            {
                var request = await httpClient.GetAsync(url).ConfigureAwait(continueOnCapturedContext);
                var content = await request.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext);

                return content;
            }
        }
    }
}

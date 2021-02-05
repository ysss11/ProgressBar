using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressBar
{
    public partial class StartProgressBar : Form
    {
        public StartProgressBar()
        {
            InitializeComponent();
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            ProgressDialog pd = new ProgressDialog("進行状況ダイアログのテスト",
                new DoWorkEventHandler(ProgressDialog_DoWork),
                100);
            //進行状況ダイアログを表示する
            DialogResult result = pd.ShowDialog();
            //結果を取得する
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show("キャンセルされました",
                    "キャンセル",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2);
            }
            else if (result == DialogResult.Abort)
            {
                //エラー情報を取得する
                Exception ex = pd.Error;
                MessageBox.Show("エラー: " + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (result == DialogResult.OK)
            {
                //結果を取得する
                int stopTime = (int)pd.Result;
                MessageBox.Show("成功しました: " + stopTime.ToString() + "ms",
                    "success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }

            //後始末
            pd.Dispose();
        }

        private void ProgressDialog_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;

            //パラメータを取得する
            int stopTime = (int)e.Argument;

            //時間のかかる処理を開始する
            for (int i = 1; i <= 100; i++)
            {
                //キャンセルされたか調べる
                if (bw.CancellationPending)
                {
                    //キャンセルされたとき
                    e.Cancel = true;
                    return;
                }

                //指定された時間待機する
                System.Threading.Thread.Sleep(stopTime);

                //ProgressChangedイベントハンドラを呼び出し、
                //コントロールの表示を変更する
                bw.ReportProgress(i, i.ToString() + "% 終了しました");
            }

            //結果を設定する
            e.Result = stopTime * 100;
        }
    }
}

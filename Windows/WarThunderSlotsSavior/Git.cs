using System.Diagnostics;

public class Git {
    /// <summary>
    /// 获取环境git.ext的环境变量路径
    /// </summary>
    private static string StrEnvironmentVariable {
        get {
            string strPath = System.Environment.GetEnvironmentVariable("Path");
            if (string.IsNullOrEmpty(strPath)) {
                //">>>>>strEnvironmentVariable: enviromentVariable is not config!!!!"
                return null;
            }
            string[] strResults = strPath.Split(';');
            for (int i = 0; i < strResults.Length; i++) {
                if (!strResults[i].Contains(@"Git\cmd"))
                    continue;
                strPath = strResults[i];
            }
            return strPath;
        }
    }
    /// <summary>
    ///
    /// git工作路径
    /// </summary>
    public static string strWorkingDir;

    /// <summary>
    /// 执行git指令
    /// </summary>
    public static int ExecuteGitCommand(string strCommnad, DataReceivedEventHandler call) {
        string strGitPath = System.IO.Path.Combine(StrEnvironmentVariable, "git.exe");
        if (string.IsNullOrEmpty(strGitPath)) {
            //">>>>>strEnvironmentVariable: enviromentVariable is not config!!!!"
            return -1;
        }
        Process process = new Process();
        process.StartInfo.FileName = strGitPath;
        process.StartInfo.Arguments = strCommnad;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.WorkingDirectory = strWorkingDir;
        process.OutputDataReceived += call;
        process.OutputDataReceived -= OnOutputDataReceived;
        process.OutputDataReceived += OnOutputDataReceived;
        process.Start();
        process.BeginOutputReadLine();
        process.WaitForExit();

        return process.ExitCode;
    }
    /// <summary>
    /// 输出git指令执行结果
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void OnOutputDataReceived(object sender, DataReceivedEventArgs e) {
        if (null == e || string.IsNullOrEmpty(e.Data)) {
            //">>>>>>Git command error!!!!!"
            return;
        }
        //EventLog.Log(e.Data);
    }
}

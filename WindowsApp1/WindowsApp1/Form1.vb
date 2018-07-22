Imports System.IO
Imports System.Text

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedItem = ComboBox1.Items(0)
        LoadData()

    End Sub
    Private Sub LoadData()
        dirLabel.Text = My.Settings.ProjectsFolder
        For Each Project As String In Directory.GetDirectories(My.Settings.ProjectsFolder)
            ListBox1.Items.Add(Project)
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            My.Settings.ProjectsFolder = FolderBrowserDialog1.SelectedPath
            LoadData()
        End If
    End Sub
    Private Function GetProjectName(PPath As String) As String
        Try
            Dim strarr() As String = PPath.Split("\"c)
            Dim ProjectName As String = strarr(strarr.Length - 1)
            Return ProjectName
        Catch e As NullReferenceException
            Throw e
        End Try
    End Function
    Private Function GetExecutablePath(PPath As String) As String
        Dim executablePath As New StringBuilder
        executablePath.Append(PPath)
        If ComboBox1.SelectedItem = "JetBrains CLion" Then
            executablePath.Append("\cmake-build-debug\")
            executablePath.Append(GetProjectName(PPath))
            executablePath.Append(".exe")
        End If
        Return executablePath.ToString
    End Function
    Private Sub StartProject()
        Dim SelectedProject As String = ListBox1.SelectedItem.ToString
        StartProgram(GetExecutablePath(SelectedProject), GetProjectName(SelectedProject))
    End Sub
    Private Sub StartProgram(path As String, title As String)
        Process.Start("cmd", "/c title " + title + " && " + path + " && echo. && pause")
    End Sub
    Private Sub ExportApp(PPath As String)
        Try
            Dim Path As New StringBuilder
            Dim PName As String = GetProjectName(PPath)
            If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
                Path.Append(FolderBrowserDialog1.SelectedPath)
            End If
            Path.Append("\")
            Path.Append(PName)
            If (Not Directory.Exists(Path.ToString)) Then
                Directory.CreateDirectory(Path.ToString)
            End If
            Dim destExecutable As String = Path.ToString + "\" + PName + ".exe"
            File.Copy(GetExecutablePath(PPath), destExecutable, True)
            'Create Batch Launcher
            Dim fs As FileStream = File.Create(Path.ToString + "\Launch.bat")
            Dim txt As Byte() = New UTF8Encoding(True).GetBytes("@echo off" + vbNewLine + "title " + PName + vbNewLine + destExecutable + vbNewLine + "echo." + vbNewLine + "pause")
            fs.Write(txt, 0, txt.Length)
            fs.Close()
            MessageBox.Show("Your Project has been successfully exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As NullReferenceException
            MessageBox.Show("Please select a project", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Private Sub startBtn_Click(sender As Object, e As EventArgs) Handles startBtn.Click
        StartProject()
    End Sub

    Private Sub exportBtn_Click(sender As Object, e As EventArgs) Handles exportBtn.Click
        ExportApp(ListBox1.SelectedItem)
    End Sub
End Class

Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports Microsoft.Win32

''''''''''''''Proble: quando a senha esta errada o aplicatico continua duplicando 
''''''''''''''o arquivo


Public Class FormPrincipal
    Inherits System.Windows.Forms.Form

    'Cria um Array de 8 - byte para a chave privada
    'Array inicia do zero
    Public Achave(7) As Byte
    Public caminhoArquivo As String     'o caminho menos a extensão

    'Preenche o vetor de inicialização com alguns valores aleatórios
    Private Vector() As Byte = {&H12, &H44, &H16, &HEE, &H88, &H15, &HDD, &H41}


    Private Sub FormPrincipal_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load


    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
    Sub Cifrar(ByVal inName As String, ByVal outName As String)

        Try
            'cria um buffers
            Dim storage(4096) As Byte
            'bytes escritos
            Dim totalBytesEscritos As Long = 8

            Dim tamanhoPacote As Integer
            'determina o numero de bytes escritos de uma vez

            'Declara os arquivos streams
            Dim ArqEntrada As New FileStream(inName, FileMode.Open, FileAccess.Read)
            Dim ArqSaida As New FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write)
            ArqSaida.SetLength(0)

            Dim ComprimentoTotalArquivo As Long = ArqEntrada.Length

            'CRia um objeto cripto
            Dim DesCrip As New DESCryptoServiceProvider
            Dim CrStream As New CryptoStream(ArqSaida, DesCrip.CreateEncryptor(Achave, Vector), CryptoStreamMode.Write)

            'Fluxo de stream
            While totalBytesEscritos < ComprimentoTotalArquivo
                tamanhoPacote = ArqEntrada.Read(storage, 0, 4096)
                CrStream.Write(storage, 0, tamanhoPacote)
                totalBytesEscritos = Convert.ToInt32(totalBytesEscritos + tamanhoPacote / DesCrip.BlockSize * DesCrip.BlockSize)

                

            End While

            'usando progresbar
            For Cont = 1 To 100
                ProgressBar1.Value = Cont
            Next

            CrStream.Close()


        Catch ex As Exception
            MsgBox(ex.Message)


        End Try

        LblMessagem.Text = (" Cifragem de arquivos encerrada...")

        MessageBox.Show("Agora você tem os 2 arquivos, o original e o cifrado.Não esqueça sua senha para descifrar o arquivo, anote agora == SENHA ==> " & TextBox2.Text, "LockCad", MessageBoxButtons.OK, MessageBoxIcon.Warning)



        Me.Close()

    End Sub

    Private Sub CriaChave(ByVal strKey As String)

        'Array  de Bytes para tratar a senha
        Dim ArrByte(7) As Byte

        Dim AscEncod As New ASCIIEncoding
        Dim I As Integer = 0
        AscEncod.GetBytes(strKey, I, strKey.Length, ArrByte, I)

        'Obtem o valor do Hash da senha
        Dim Hasha As New SHA1CryptoServiceProvider
        Dim ArrHash() As Byte = Hasha.ComputeHash(ArrByte)

        'Poe o valor do Hasha na chave
        For I = 0 To 7
            Achave(I) = ArrHash(I)

        Next I



    End Sub
    Sub Decifrar(ByVal inName As String, ByVal outname As String)
        Try

            Dim Storage(4096) As Byte
            Dim TotalBytesEscritos As Long = 8
            Dim tamanhoPacote As Integer

            Dim ArqEntrada As New FileStream(inName, FileMode.Open, FileAccess.Read)
            Dim ArqSaida As New FileStream(outname, FileMode.OpenOrCreate, FileAccess.Write)

            ArqSaida.SetLength(0)

            Dim ComprimentoTotalArquivo As Long = ArqEntrada.Length
            'instancia um objeto para cifrar
            Dim Des As New DESCryptoServiceProvider
            Dim crStream As New CryptoStream(ArqSaida, Des.CreateDecryptor(Achave, Vector), CryptoStreamMode.Write)

            Dim Ex As Exception

            While TotalBytesEscritos < ComprimentoTotalArquivo
                tamanhoPacote = ArqEntrada.Read(Storage, 0, 4096)
                crStream.Write(Storage, 0, tamanhoPacote)
                TotalBytesEscritos = Convert.ToInt32(TotalBytesEscritos + tamanhoPacote / Des.BlockSize * Des.BlockSize)
                Console.WriteLine("Processed {0 } bytes,{1} bytes total", tamanhoPacote, TotalBytesEscritos)


            End While
            For cont = 1 To 100
                ProgressBar1.Value = cont


            Next
            crStream.Close()



        Catch ex As Exception

            MsgBox(ex.Message & " SENHA INCORRETA.", MsgBoxStyle.Critical)



            'Fecha o form
            Me.Close()


        End Try
        'desabilitar botões
        TxtProcurar.Enabled = False
        TxtCifragem.Enabled = False

        MessageBox.Show("Agora você tem o arquivo original Descifrado ", "LockCad", MessageBoxButtons.OK, MessageBoxIcon.Warning)


        Me.Close()

    End Sub


    Private Sub TxtProcurar_Click(sender As System.Object, e As System.EventArgs) Handles TxtProcurar.Click
        OpenFileDialog1.Title = "Selecione o arquivo para Cifrar/Decifrar"
        OpenFileDialog1.Filter = "Todos os arquivos (*.*)| *.*"
        LblMessagem.Text = ""

        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = OpenFileDialog1.FileName
            TxtProcurar.Enabled = True
            TxtCifragem.Enabled = True

        End If


    End Sub

    Private Sub TabPageCriptografar_Click(sender As System.Object, e As System.EventArgs) Handles TabPageCriptografar.Click

    End Sub

    Private Sub TabPageSoftWare_Click(sender As System.Object, e As System.EventArgs) Handles TabPageSoftWare.Click

    End Sub

    Private Sub TextBox2_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TxtCifragem_Click(sender As System.Object, e As System.EventArgs) Handles TxtCifragem.Click
        Dim arquivo_destino, arquivo_origem As String

        'verifica se foi entrado um nome de arquivo
        If TextBox1.Text = "" Or TextBox2.Text = "" Then MsgBox("Você precisa informar um nome, " & " caminho " & "de arquivo e uma senha !") : Exit Sub



        'verifica se esta cifrado ou decifrado (o nome do arquivo termina com "_#")
        Dim ext As String                'extensão do arquivo

        Dim n As Integer                 'a localiza~ção do ponto no caminho do arquivo

        n = TextBox1.Text.IndexOf(".")   'se não tem extensão retora -1
        If n <> -1 Then
            ext = TextBox1.Text.Substring(n + 1)
            caminhoArquivo = TextBox1.Text.Substring(0, TextBox1.Text.Length - ext.Length - 1)
        Else
            caminhoArquivo = TextBox1.Text


        End If

        If caminhoArquivo.Substring(caminhoArquivo.Length - 2) = "_#" Then      'este arquivo sera decifrado
            'decifra
            arquivo_origem = TextBox1.Text
            'remove o "_#":
            caminhoArquivo = caminhoArquivo.Substring(0, caminhoArquivo.Length - 2)

            If ext <> "" Then caminhoArquivo &= "." & ext

            arquivo_destino = caminhoArquivo

            CriaChave(TextBox2.Text) 'cria chave
            Decifrar(arquivo_origem, arquivo_destino)
            LblMessagem.Text = "Decifragem terminada..."

           
            Exit Sub


        End If
        ' End If------------------percebi que quando incio o if apos o THEN se eu colocar o Código apos o
        'THEN  a condição não pede o endif





        'Cifrar
        'não há "_#" no final do arquivo , vamos cifrar
        arquivo_origem = TextBox1.Text
        'acrescenta o  o compose the Cifrared file's filepath by appending "_#": 
        caminhoArquivo &= "_#"
        If ext <> "" Then caminhoArquivo &= "." & ext
        arquivo_destino = caminhoArquivo
        CriaChave(TextBox2.Text)
        Cifrar(arquivo_origem, arquivo_destino)
        ''''''''''''''''''''' LblMessagem.Text = " Cifragem do Arquivo encerrada..."

        'desabilitar botões
        TxtProcurar.Enabled = False
        TxtCifragem.Enabled = False



    End Sub

    Private Sub BTDesabilita_Click(sender As Object, e As EventArgs) Handles BTDesabilita.Click
        'Ao meu ver o código esta perfeito, o que faz a difereça nop abilitar ou desabilitar
        'é o seguinte trech  regKey.SetValue("NoControlPanel", 0) onde o valor 1 do objeto desabilita
        'e o valor 0 abilita

        Dim regKey As RegistryKey

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies", True)

        regKey.CreateSubKey("Explorer")

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", True)

        regKey.SetValue("NoControlPanel", 1)

        regKey.Close()

        If LblMesnsagem2.Text = "Painel de Controle esta Abilitado!" Then
            LblMesnsagem2.Text = ""
        End If
        LblMesnsagem.Text = "Painel de Controle esta Desabilitado!"
        LblMesnsagem.ForeColor = Color.Red

        LblAviso.Text = "Reinicie o computador"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Ao meu ver o código esta perfeito, o que faz a difereça nop abilitar ou desabilitar
        'é o seguinte trech  regKey.SetValue("NoControlPanel", 0) onde o valor 1 do objeto desabilita
        'e o valor 0 abilita

        Dim regKey As RegistryKey

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies", True)

        regKey.CreateSubKey("Explorer")

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", True)

        regKey.SetValue("NoControlPanel", 0)

        regKey.Close()

        If LblMesnsagem.Text = "Painel de Controle esta Desabilitado!" Then
            LblMesnsagem.Text = "."
        End If
        LblMesnsagem2.Text = "Painel de Controle esta Abilitado!"
        LblMesnsagem2.ForeColor = Color.BlueViolet

        LblAviso.Text = "Reinicie o computador"

    End Sub

    Private Sub FormPrincipal_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim F As Form = sender
        If F.WindowState = FormWindowState.Minimized Then
            Me.Hide()
            NotifyIcon1.Visible = True

        End If
    End Sub

    Private Sub ABRIRLockCadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ABRIRLockCadToolStripMenuItem.Click
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False


    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False






    End Sub
End Class

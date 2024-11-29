using System;
using System.Windows.Forms;
using Npgsql;

public static class Db
{

    private static string connectionString = "Host=seu_host;Port=5432;Username=postgres;Password=sua_senha;Database=postgres;Timeout=30;CommandTimeout=30;SSLMode=Require;Trust Server Certificate=true";

    private static NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }


    public static void CadastrarOuAtualizarProduto(string titulo, int codigo)
    {
        try
        {
            using (var connection = GetConnection())
            {
                connection.Open(); 

                string checkQuery = "SELECT COUNT(*) FROM produtos WHERE codigo = @codigo";
                using (var checkCmd = new NpgsqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("codigo", codigo);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        string updateQuery = "UPDATE produtos SET titulo = @titulo WHERE codigo = @codigo";
                        using (var updateCmd = new NpgsqlCommand(updateQuery, connection))
                        {
                            updateCmd.Parameters.AddWithValue("titulo", titulo);
                            updateCmd.Parameters.AddWithValue("codigo", codigo);
                            updateCmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO produtos (titulo, codigo) VALUES (@titulo, @codigo)";
                        using (var insertCmd = new NpgsqlCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("titulo", titulo);
                            insertCmd.Parameters.AddWithValue("codigo", codigo);
                            insertCmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        catch (NpgsqlException npgsqlEx)
        {
            MessageBox.Show($"Erro de banco de dados: {npgsqlEx.Message}\nStack Trace: {npgsqlEx.StackTrace}",
                            "Erro de Banco de Dados",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
        catch (TimeoutException timeoutEx)
        {
            MessageBox.Show($"A conexão com o banco de dados demorou muito tempo: {timeoutEx.Message}",
                            "Erro de Conexão",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro desconhecido: {ex.Message}\n{ex.StackTrace}",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }




    public static void DeletarProduto(int codigo)
    {
        try
        {
            using (var connection = GetConnection())
            {
                connection.Open(); 

                string checkQuery = "SELECT titulo FROM produtos WHERE codigo = @codigo";
                using (var checkCmd = new NpgsqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("codigo", codigo);
                    var tituloProduto = checkCmd.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(tituloProduto))
                    {
                        var result = MessageBox.Show(
                            $"Você tem certeza que deseja excluir o produto: {tituloProduto}?",
                            "Confirmar Exclusão",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {                           
                            string deleteQuery = "DELETE FROM produtos WHERE codigo = @codigo";
                            using (var deleteCmd = new NpgsqlCommand(deleteQuery, connection))
                            {
                                deleteCmd.Parameters.AddWithValue("codigo", codigo);
                                deleteCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Produto deletado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("A exclusão foi cancelada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Produto não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        catch (NpgsqlException npgsqlEx)
        {
            MessageBox.Show($"Erro de banco de dados: {npgsqlEx.Message}\nStack Trace: {npgsqlEx.StackTrace}",
                            "Erro de Banco de Dados",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
        catch (TimeoutException timeoutEx)
        {
            MessageBox.Show($"A conexão com o banco de dados demorou muito tempo: {timeoutEx.Message}",
                            "Erro de Conexão",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
        catch (Exception ex)
        { 
            MessageBox.Show($"Erro desconhecido: {ex.Message}\n{ex.StackTrace}",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }
}

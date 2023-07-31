# CadastroFuncionarios_2

A API possui 3 camadas:

Camada Funcionarios - camada de interface responsável pela interação com usuário final, configuração dos controladores, etc.
Camada Serviço - contém os métodos de validação dos métodos de escrita das informações no banco de dados.
Camada Database - contém as classes para operações no banco de dados sql usando EF core.

Operação da API:

Os métodos GET (leitura) são liberados para qualquer usuário verificar as informações armazenadas no banco de dados.
Para as operações de escrita, é necessário a autenticação com usuário e senha com direitos administrativos (username = "admin", password = "admin"). Uma vez autenticado, é gerado um token no campo de resposta que deve ser inserido em "Authorize" para liberar as operações de PUT,POST e DELETE. O token gerado tem expiração de 30 minutos.

# 1.Projeto
Projeto: C#, .NET 8, Worker Service.
Nome: RPA Search Top 10 products Amazon
Tipo: Rest Request.
É uma automação que faz pesquisas de produtos na Amazon, extrai os 10 primeiros resultados de cada busca (incluindo nome, preço, e link do produto), e armazena essas informações em um banco de dados PostgreSQL. Ele utiliza o Worker Service do .NET para rodar em segundo plano e interage com APIs e arquivos Excel para obter as palavras-chave da pesquisa e salvar os dados.

# 2.Modelo de Entidade e Relacionamento (MER)
Estrutura de tabelas para criação do banco de dados.

![MER'S.png](https://github.com/user-attachments/assets/e50e75bf-29fd-49ef-8600-f284b1f6b05b)

# 3.Arquitetura do projeto
- **Worker.cs:** Classe principal para a execução do fluxo do robô.
- **Program.cs:** Classe de entrada do sistema, responsável por inicializar a aplicação, criar serviços, configuração para windows service.

- **Handlers:** Diretório que contém a manipulação da plataforma em geral, por exemplo, seja por requests, selenium, terminais. Aqui concentramos o código que envolve ações dentro do sistema que estamos automatizando. Ao criar uma classe, escreva no final o sufixo Handler. Exemplos: SeleniumHandler, NavigationHandler.
- **Utils:** Classes com códigos para manipulação, tratamento de dados, por exemplo, formatação de CPF, remoção de caracteres específicos, entre outros.
- **Models:** Diretório que contém as classes de modelagem. Exemplos: Lote, Item, Usuário.
- **Repository:** Diretório que contém as classes para acesso ao banco de dados. Exemplos: LoteRepository.cs, ItemRepository.cs, UsuarioRepository.cs. 
Utilizar a injeção de dependência, criando um serviço na Program.cs para as classes de repositório.

# 4.Desenvolvimento
Aqui está descrito o fluxo de desenvolvimento.

## 4.1. Desenvolvimento Geral

Criar o fluxo completo, previsto no escopo.
Criar as classes de repositório, como leitura, criação, atualização dos dados no banco.
Fazer a persistências dos dados no banco a cada item consultado pelo robô.

## 5.Scopo
![image](https://github.com/user-attachments/assets/b31993f9-7a58-43e4-a2fa-a93f55c44be6)

(miro) [https://miro.com/welcomeonboard/T21LSHpJbGRtV2FWUjdRUWFOZktCbWNmMEl4ZktseTZzMDFxU3dUWVBXSThQSE94dlpjRUNNUnAxYlRONDY3QnwzNDU4NzY0NjAzNDgzNzg4MTA5fDI=?share_link_id=668088493368]


# Projeto
Projeto: C#, .NET 8, Worker Service.
Nome: RPA Search Top 10 products Amazon
Tipo: Rest Request.
É uma automação que faz pesquisas de produtos na Amazon, extrai os 10 primeiros resultados de cada busca (incluindo nome, preço, e link do produto), e armazena essas informações em um banco de dados PostgreSQL. Ele utiliza o Worker Service do .NET para rodar em segundo plano e interage com APIs e arquivos Excel para obter as palavras-chave da pesquisa e salvar os dados.

# Modelo de Entidade e Relacionamento (MER)
Estrutura de tabelas para criação do banco de dados.

![MER'S.png](/.attachments/MER'S-8ac0d2ca-452a-4303-ad52-55232aef3230.png)

# Arquitetura do projeto
**Worker.cs:** Classe principal para a execução do fluxo do robô.
Program.cs: Classe de entrada do sistema, responsável por inicializar a aplicação, criar serviços, configuração para windows service.

**Handlers:** Diretório que contém a manipulação da plataforma em geral, por exemplo, seja por requests, selenium, terminais. Aqui concentramos o código que envolve ações dentro do sistema que estamos automatizando. Ao criar uma classe, escreva no final o sufixo Handler. Exemplos: SeleniumHandler, NavigationHandler.
Utils: Classes com códigos para manipulação, tratamento de dados, por exemplo, formatação de CPF, remoção de caracteres específicos, entre outros.
Models: Diretório que contém as classes de modelagem. Exemplos: Lote, Item, Usuário.
Repository: Diretório que contém as classes para acesso ao banco de dados. Exemplos: LoteRepository.cs, ItemRepository.cs, UsuarioRepository.cs. 
Utilizar a injeção de dependência, criando um serviço na Program.cs para as classes de repositório.

# Desenvolvimento
Aqui está descrito o fluxo de desenvolvimento.

## 4.1. Desenvolvimento Geral

Criar o fluxo completo, previsto no escopo.
Criar as classes de repositório, como leitura, criação, atualização dos dados no banco.
Fazer a persistências dos dados no banco a cada item consultado pelo robô.

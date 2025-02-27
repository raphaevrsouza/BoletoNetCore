﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BoletoNetCore.Testes
{
    [TestFixture]
    [Category("Sicredi Carteira 1 Var A")]
    public class BancoSicrediCarteira1Tests
    {
        readonly IBanco _banco;
        public BancoSicrediCarteira1Tests()
        {
            var contaBancaria = new ContaBancaria
            {
                Agencia = "0156",
                Conta = "85305",
                DigitoConta = "4",
                CarteiraPadrao = "1",
                TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                VariacaoCarteiraPadrao = "A",
                TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa,
                OperacaoConta = "05"

            };
            _banco = Banco.Instancia(Bancos.Sicredi);
            _banco.Beneficiario = TestUtils.GerarBeneficiario("85305", "", "", contaBancaria);
            _banco.FormataBeneficiario();
        }

        [Test]
        public void Sicredi_1_REM400()
        {
            TestUtils.TestarHomologacao(_banco, TipoArquivo.CNAB400, nameof(BancoSicrediCarteira1Tests), 9, true, "N", 00001);
        }

        [Test]
        public void Sicredi_1_REM240()
        {
            TestUtils.TestarHomologacao(_banco, TipoArquivo.CNAB240, nameof(BancoSicrediCarteira1Tests), 9, true, "N", 00001);
        }

        [TestCase(500, "4", "4", "4", "19/200004-8", "74894629800000500001119200004801560585305103", "74891.11927 00004.801569 05853.051034 4 62980000050000", 2015, 01, 04)]
        [TestCase(400, "3", "4", "4", "19/200003-0", "74894740700000400001119200003001560585305101", "74891.11927 00003.001567 05853.051018 4 74070000040000", 2018, 01, 17)]
        [TestCase(800, "10", "10", "3", "19/200010-2", "74893787000000800001119200010201560585305102", "74891.11927 00010.201564 05853.051026 3 78700000080000", 2019, 04, 25)]
        [TestCase(900, "9", "9", "6", "19/200009-9", "74896787700000900001119200009901560585305100", "74891.11927 00009.901562 05853.051000 6 78770000090000", 2019, 05, 02)]
        [TestCase(603.56, "5", "5", "6", "19/200005-6", "74896790500000603561119200005601560585305109", "74891.11927 00005.601562 05853.051091 6 79050000060356", 2019, 05, 30)]
        [TestCase(300, "2", "2", "7", "19/200002-1", "74897790500000300001119200002101560585305102", "74891.11927 00002.101566 05853.051026 7 79050000030000", 2019, 05, 30)]
        [TestCase(200, "6", "6-1/3", "5", "19/200006-4", "74895810800000200001119200006401560585305104", "74891.11927 00006.401566 05853.051042 5 81080000020000", 2019, 12, 19)]
        [TestCase(200, "7", "6-2/3", "8", "19/200007-2", "74898813900000200001119200007201560585305100", "74891.11927 00007.201569 05853.051000 8 81390000020000", 2020, 01, 19)]
        [TestCase(200, "8", "6-3/3", "4", "19/200008-0", "74894817000000200001119200008001560585305105", "74891.11927 00008.001562 05853.051059 4 81700000020000", 2020, 02, 19)]
        public void Deve_criar_boleto_Sicredi_1_var_A_com_digito_verificador_valido(decimal valorTitulo, string nossoNumero, string numeroDocumento, string digitoVerificador, string nossoNumeroFormatado, string codigoDeBarras, string linhaDigitavel, params int[] anoMesDia)
        {
            var boleto = new Boleto(_banco)
            {
                DataVencimento = new DateTime(anoMesDia[0], anoMesDia[1], anoMesDia[2]),
                DataEmissao = new DateTime(2019, DateTime.Now.Month, DateTime.Now.Day),
                ValorTitulo = valorTitulo,
                NossoNumero = nossoNumero,
                NumeroDocumento = numeroDocumento,
                EspecieDocumento = TipoEspecieDocumento.DMI,
                Pagador = TestUtils.GerarPagador()
            };

            //Ação
            boleto.ValidarDados();

            //Assertivas
            Assert.That(boleto.CodigoBarra.DigitoVerificador, Is.EqualTo(digitoVerificador), $"Dígito Verificador diferente de {digitoVerificador}");
        }

        [TestCase(500, "4", "4", "4", "19/200004-8", "74894629800000500001119200004801560585305103", "74891.11927 00004.801569 05853.051034 4 62980000050000", 2015, 01, 04)]
        [TestCase(400, "3", "4", "4", "19/200003-0", "74894740700000400001119200003001560585305101", "74891.11927 00003.001567 05853.051018 4 74070000040000", 2018, 01, 17)]
        [TestCase(800, "10", "10", "3", "19/200010-2", "74893787000000800001119200010201560585305102", "74891.11927 00010.201564 05853.051026 3 78700000080000", 2019, 04, 25)]
        [TestCase(900, "9", "9", "6", "19/200009-9", "74896787700000900001119200009901560585305100", "74891.11927 00009.901562 05853.051000 6 78770000090000", 2019, 05, 02)]
        [TestCase(603.56, "5", "5", "6", "19/200005-6", "74896790500000603561119200005601560585305109", "74891.11927 00005.601562 05853.051091 6 79050000060356", 2019, 05, 30)]
        [TestCase(300, "2", "2", "7", "19/200002-1", "74897790500000300001119200002101560585305102", "74891.11927 00002.101566 05853.051026 7 79050000030000", 2019, 05, 30)]
        [TestCase(200, "6", "6-1/3", "5", "19/200006-4", "74895810800000200001119200006401560585305104", "74891.11927 00006.401566 05853.051042 5 81080000020000", 2019, 12, 19)]
        [TestCase(200, "7", "6-2/3", "8", "19/200007-2", "74898813900000200001119200007201560585305100", "74891.11927 00007.201569 05853.051000 8 81390000020000", 2020, 01, 19)]
        [TestCase(200, "8", "6-3/3", "4", "19/200008-0", "74894817000000200001119200008001560585305105", "74891.11927 00008.001562 05853.051059 4 81700000020000", 2020, 02, 19)]
        public void Deve_criar_boleto_Sicredi_1_var_A_com_nosso_numero_formatado_valido(decimal valorTitulo, string nossoNumero, string numeroDocumento, string digitoVerificador, string nossoNumeroFormatado, string codigoDeBarras, string linhaDigitavel, params int[] anoMesDia)
        {
            //Ambiente
            //Testes do Nosso Numero (2 primeiros digitos yy ) Fixo em 2019 -> Data Emissâo com Ano de 2019
            var boleto = new Boleto(_banco)
            {
                DataVencimento = new DateTime(anoMesDia[0], anoMesDia[1], anoMesDia[2]),
                DataEmissao = new DateTime(2019, DateTime.Now.Month, DateTime.Now.Day),
                ValorTitulo = valorTitulo,
                NossoNumero = nossoNumero,
                NumeroDocumento = numeroDocumento,
                EspecieDocumento = TipoEspecieDocumento.DMI,
                Pagador = TestUtils.GerarPagador()
            };

            //Ação
            boleto.ValidarDados();

            //Assertivas 
            Assert.That(boleto.NossoNumeroFormatado, Is.EqualTo(nossoNumeroFormatado), "Nosso número inválido");
        }

        [TestCase(500, "4", "4", "4", "19/200004-8", "74894629800000500001119200004801560585305103", "74891.11927 00004.801569 05853.051034 4 62980000050000", 2015, 01, 04)]
        [TestCase(400, "3", "4", "4", "19/200003-0", "74894740700000400001119200003001560585305101", "74891.11927 00003.001567 05853.051018 4 74070000040000", 2018, 01, 17)]
        [TestCase(800, "10", "10", "3", "19/200010-2", "74893787000000800001119200010201560585305102", "74891.11927 00010.201564 05853.051026 3 78700000080000", 2019, 04, 25)]
        [TestCase(900, "9", "9", "6", "19/200009-9", "74896787700000900001119200009901560585305100", "74891.11927 00009.901562 05853.051000 6 78770000090000", 2019, 05, 02)]
        [TestCase(603.56, "5", "5", "6", "19/200005-6", "74896790500000603561119200005601560585305109", "74891.11927 00005.601562 05853.051091 6 79050000060356", 2019, 05, 30)]
        [TestCase(300, "2", "2", "7", "19/200002-1", "74897790500000300001119200002101560585305102", "74891.11927 00002.101566 05853.051026 7 79050000030000", 2019, 05, 30)]
        [TestCase(200, "6", "6-1/3", "5", "19/200006-4", "74895810800000200001119200006401560585305104", "74891.11927 00006.401566 05853.051042 5 81080000020000", 2019, 12, 19)]
        [TestCase(200, "7", "6-2/3", "8", "19/200007-2", "74898813900000200001119200007201560585305100", "74891.11927 00007.201569 05853.051000 8 81390000020000", 2020, 01, 19)]
        [TestCase(200, "8", "6-3/3", "4", "19/200008-0", "74894817000000200001119200008001560585305105", "74891.11927 00008.001562 05853.051059 4 81700000020000", 2020, 02, 19)]
        public void Deve_criar_boleto_Sicredi_1_var_A_com_codigo_de_barras_valido(decimal valorTitulo, string nossoNumero, string numeroDocumento, string digitoVerificador, string nossoNumeroFormatado, string codigoDeBarras, string linhaDigitavel, params int[] anoMesDia)
        {
            var boleto = new Boleto(_banco)
            {
                DataVencimento = new DateTime(anoMesDia[0], anoMesDia[1], anoMesDia[2]),
                DataEmissao = new DateTime(2019, DateTime.Now.Month, DateTime.Now.Day),
                ValorTitulo = valorTitulo,
                NossoNumero = nossoNumero,
                NumeroDocumento = numeroDocumento,
                EspecieDocumento = TipoEspecieDocumento.DMI,
                Pagador = TestUtils.GerarPagador()
            };

            //Ação
            boleto.ValidarDados();

            //Assertivas 
            Assert.That(boleto.CodigoBarra.CodigoDeBarras, Is.EqualTo(codigoDeBarras), "Código de Barra inválido");
        }

        [TestCase(500, "4", "4", "4", "19/200004-8", "74894629800000500001119200004801560585305103", "74891.11927 00004.801569 05853.051034 4 62980000050000", 2015, 01, 04)]
        [TestCase(400, "3", "4", "4", "19/200003-0", "74894740700000400001119200003001560585305101", "74891.11927 00003.001567 05853.051018 4 74070000040000", 2018, 01, 17)]
        [TestCase(800, "10", "10", "3", "19/200010-2", "74893787000000800001119200010201560585305102", "74891.11927 00010.201564 05853.051026 3 78700000080000", 2019, 04, 25)]
        [TestCase(900, "9", "9", "6", "19/200009-9", "74896787700000900001119200009901560585305100", "74891.11927 00009.901562 05853.051000 6 78770000090000", 2019, 05, 02)]
        [TestCase(603.56, "5", "5", "6", "19/200005-6", "74896790500000603561119200005601560585305109", "74891.11927 00005.601562 05853.051091 6 79050000060356", 2019, 05, 30)]
        [TestCase(300, "2", "2", "7", "19/200002-1", "74897790500000300001119200002101560585305102", "74891.11927 00002.101566 05853.051026 7 79050000030000", 2019, 05, 30)]
        [TestCase(200, "6", "6-1/3", "5", "19/200006-4", "74895810800000200001119200006401560585305104", "74891.11927 00006.401566 05853.051042 5 81080000020000", 2019, 12, 19)]
        [TestCase(200, "7", "6-2/3", "8", "19/200007-2", "74898813900000200001119200007201560585305100", "74891.11927 00007.201569 05853.051000 8 81390000020000", 2020, 01, 19)]
        [TestCase(200, "8", "6-3/3", "4", "19/200008-0", "74894817000000200001119200008001560585305105", "74891.11927 00008.001562 05853.051059 4 81700000020000", 2020, 02, 19)]
        public void Deve_criar_boleto_Sicredi_1_var_A_com_linha_digitavel_valida(decimal valorTitulo, string nossoNumero, string numeroDocumento, string digitoVerificador, string nossoNumeroFormatado, string codigoDeBarras, string linhaDigitavel, params int[] anoMesDia)
        {
            var boleto = new Boleto(_banco)
            {
                DataVencimento = new DateTime(anoMesDia[0], anoMesDia[1], anoMesDia[2]),
                DataEmissao = new DateTime(2019, DateTime.Now.Month, DateTime.Now.Day),
                ValorTitulo = valorTitulo,
                NossoNumero = nossoNumero,
                NumeroDocumento = numeroDocumento,
                EspecieDocumento = TipoEspecieDocumento.DMI,
                Pagador = TestUtils.GerarPagador()
            };

            //Ação
            boleto.ValidarDados();

            //Assertivas 
            Assert.That(boleto.CodigoBarra.LinhaDigitavel, Is.EqualTo(linhaDigitavel), "Linha digitável inválida");
        }




        #region Teste de producao
        [TestCase(500, "4", "4", "4", "19/200004-8", "74894629800000500001119200004801560585305103", "74891.11927 00004.801569 05853.051034 4 62980000050000", 2015, 01, 04)]
        [TestCase(400, "3", "4", "4", "19/200003-0", "74894740700000400001119200003001560585305101", "74891.11927 00003.001567 05853.051018 4 74070000040000", 2018, 01, 17)]
        [TestCase(800, "10", "10", "3", "19/200010-2", "74893787000000800001119200010201560585305102", "74891.11927 00010.201564 05853.051026 3 78700000080000", 2019, 04, 25)]
        [TestCase(900, "9", "9", "6", "19/200009-9", "74896787700000900001119200009901560585305100", "74891.11927 00009.901562 05853.051000 6 78770000090000", 2019, 05, 02)]
        [TestCase(603.56, "5", "5", "6", "19/200005-6", "74896790500000603561119200005601560585305109", "74891.11927 00005.601562 05853.051091 6 79050000060356", 2019, 05, 30)]
        [TestCase(300, "2", "2", "7", "19/200002-1", "74897790500000300001119200002101560585305102", "74891.11927 00002.101566 05853.051026 7 79050000030000", 2019, 05, 30)]
        [TestCase(200, "6", "6-1/3", "5", "19/200006-4", "74895810800000200001119200006401560585305104", "74891.11927 00006.401566 05853.051042 5 81080000020000", 2019, 12, 19)]
        [TestCase(200, "7", "6-2/3", "8", "19/200007-2", "74898813900000200001119200007201560585305100", "74891.11927 00007.201569 05853.051000 8 81390000020000", 2020, 01, 19)]
        [TestCase(200, "8", "6-3/3", "4", "19/200008-0", "74894817000000200001119200008001560585305105", "74891.11927 00008.001562 05853.051059 4 81700000020000", 2020, 02, 19)]
        public async Task Sicredi_Online_RegistroBoleto_NecessarioInformar_ChaveMaster_Sicredi_Testar_Com_Dados_de_Producao(decimal valorTitulo, string nossoNumero, string numeroDocumento, string digitoVerificador, string nossoNumeroFormatado, string codigoDeBarras, string linhaDigitavel, params int[] anoMesDia)
        {
            /*
            var bancoApi = (IBancoOnlineRest)this._banco;

            // informar a chave master sicredi
            // esta chave é obtida no portal do sicredi, por meio do menu Cobrança, sub menu lateral Código de Acesso / Gerar
            bancoApi.ChaveApi = "XXXXXXXXXXXXXXXXXXXXXXXXX";

            var boleto = new Boleto(_banco)
            {
                DataVencimento = new DateTime(anoMesDia[0], anoMesDia[1], anoMesDia[2]),
                DataEmissao = new DateTime(2019, DateTime.Now.Month, DateTime.Now.Day),
                ValorTitulo = valorTitulo,
                NossoNumero = nossoNumero,
                NumeroDocumento = numeroDocumento,
                EspecieDocumento = TipoEspecieDocumento.DMI,
                Pagador = TestUtils.GerarPagador()
            };

            // atualmente a api do sicredi exige a infomação do telefone do pagador caso nao seja informado o codigo do pagador
            boleto.Pagador.Telefone = "9999999999";
            boleto.ValidarDados();

            // chave de transacao sicredi
            // aqui pode ser informado, enquanto estiver valido, o token gerado anteriormente
            // bancoApi.Token = "XXXXXXXXXXXXXXXXXXXXXXX";

            // caso nao tenha o token anteior, ou ja esteja expirado, pode-se gerar um novo
            await bancoApi.GerarToken();

            await bancoApi.RegistrarBoleto(boleto);
            */

            // essa linha apenas retira o warning referente ao async
            // ao descomentar o codigo acima, ela não é necessária
            await Task.FromResult(0);

            Assert.True(true, "Necessário descomentar o codigo acima e informar os dados bancários de produção para rodar o teste real");
        }
        #endregion

    }
}
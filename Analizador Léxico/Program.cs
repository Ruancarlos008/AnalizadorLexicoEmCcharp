﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


		// 1º ENTRADA DE TOKENS LIMITADA A 10.

internal class AnalizadorLexicoPrimitivo
{
	private static void Main(string[] args)
	{
		Console.Write("insira os dados para análise: "); // imprime na tela essa mensagem
		Console.Out.Flush(); // limpar o buffer de saída do console e garante que todas as informações que foram gravadas no buffer sejam enviadas imediatamente

		string input = Console.ReadLine();  // inserir os dados por digitação
		string entrada = input.Length > 10 ? input.Substring(0, 10) : input; // limita a string de entrada aos 10 primeiros tokens atômicos caso haja mais de 10 inseridos


		Console.Write("\nVocê inseriu esses caracteres: "); // escreve essa frase na tela
		Console.WriteLine(entrada); // imprime os dados de entrada limitados da variável entrada (serve para saber se realmente armazenou somente os 10 primeiros caracteres).
		Console.Out.Flush();

		// 2º COMPARAÇÃO DOS TOKENS DIGITADOS COM A GRAMÁTICA.

		Regex gramatica = new Regex("[a-z-A-Z0-9(-+-/@#!{}\\[\\]]"); // utiliza o método Regex (uma expressão regular) para atribuir a variável "gramatica" todos os tokens válidos da gramática especificada na avaliação.
		string invalidos = ""; // string para empilhar caracteres inválidos caso localizados no loop.
		foreach (char cont in entrada) // loop for, serve para percorrer os 10 tokens da variável "entrada"
		{
			if (!gramatica.IsMatch(cont.ToString())) // compara cada token da variável "entrada" com cada token da variável "gramatica", caso não pertença, empilha o token na variável "invados" e testa o proximo token.
			{
				invalidos += cont; // empilha o token.
			}
		}

		if (invalidos.Length > 0) // verifica se foi encontrado algum token fora da gramática.
		{
			Console.WriteLine($"O(s) tokens(s) atômico(s) > '{invalidos}' < não pertence(m) à gramática."); // imprime na tela os tokens que não fazem parte da gramática.
			Environment.Exit(0); // caso seja encontrado algum token fora da gramática, encerra o programa.
		}
		else
		{
			Console.WriteLine("\nTodos os tokens inseridos pertencem à gramática.\n"); // caso não seja encontrado nenhum token fora da gramática imprime essa mensagem na tela.
		}

		// 3º ANALISA SE A STRING É UMA PALAVRA RESERVADA DO SISTEMA.

		if (entrada[0] >= '0' && entrada[0] <= '9') // analisa se o primeiro token da variável "entrada" é numérico.
		{
			Console.WriteLine("\nPalavras iniciadas com números são palavras reservadas do sistema.\n"); // caso seja numérico, imprime esta mensagem.
			Environment.Exit(0); // encerra o programa.
		}

		//4º VERIFICA SE SÓ CONTEM CARACTERES ALFABÉTICOS NÃO RESERVADOS.

		Regex alfabetico = new Regex("[a-su-vA-SU-V]"); // será usado para verificar se só foi digitado caracteres alfabéticos não reservados.
		string invalidos2 = ""; // string para empilhar caracteres inválidos caso localizados no loop.
		foreach (char cont in entrada)

			if (!alfabetico.IsMatch(cont.ToString())) // compara cada token da variável "entrada" com cada token da variável "gramatica", caso não pertença, empilha o token na variável "invados" e testa o proximo token.
			{
				invalidos2 += cont; // empilha o token.
			}
		if (invalidos2.Length == 0) // verifica se foi encontrado algum caractere não alfabético.
		{
			Console.WriteLine("\n Todos os caracteres inseridos são alfabéticos."); // imprime a mensagem.
			Environment.Exit(0); // encerra o programa
		}

		//5º ANALISA SE É UMA EXPRESSÃO MATEMÁTICA

		List<string> OpMatematicos = new List<string>(new string[] { "x", "X", "y", "Y", "z", "Z", "w", "W", "t", "T" }); // cria uma lista os tokens reservados para expressão matemática, e salva na variável "OpMatematica".
		List<string> CarEspeciais = new List<string>(new string[] {"+", "-", "*", "/", "@", "#", "!", "(", ")", "{", "}","[", "]","0", "1", "2",
			"3", "4", "5", "6", "7", "8", "9"}); // cria uma lista todos os tokens caracteres especiais e operadores matemáticos e salva na variável "CarEspeciais"



		bool CarValidos = true; // atribui a variável "CarValidos" o valor "true" (verdadeiro), para o controle o loop for, que irá localizar a posição das variáveis das listas acima.
		for (int i = 0; i < entrada.Length - 1; i++) // loop para percorrer os tokens da variável "entrada" e verificar se estão na posição correta.
		{
			if (OpMatematicos.Contains(entrada[i].ToString())) // vai verificar se algum token da lista OpMatematica contem em alguma das 10 posições da variável "entrada".
			{
				if (!CarEspeciais.Contains(entrada[i + 1].ToString())) // caso localize algum token na vairável "entrada", irá verificar se o proximo token será um token da lista "CarEspeciais" (i+1).
				{
					CarValidos = false; // caso o próximo token não seja da lista "CarEspeciais", atribuirá "false" a variável "CarValidos".
					break; // irá sair do loop.
				}
			}
			else if (CarEspeciais.Contains(entrada[i].ToString())) // vai verificar se algum token da lista CarEspeciais contem em alguma das 10 posições da variável "entrada".
			{
				if (!OpMatematicos.Contains(entrada[i + 1].ToString())) // caso localize algum token na vairável "entrada", irá verificar se o proximo token será um token da lista "OpMatematicos"(i + 1).
				{
					CarValidos = false; // caso o próximo token não seja da lista "OpMatematico", atribuirá "false" a variável "CarValidos".
					break; // irá sair do loop.
				}
			}
		}
		if (!CarValidos) // se não encotrar tokens da lista "OpMatematicos" alternados de um elemento da lista "CarEspeciais" (ex: x+y-z(x*t)) o programa irá exibir a mensagem a baixo e irá encerrar. 
		{
			Console.WriteLine("\nExpressão matemática inválida, verifique se digitou tokens atômicos reservados x y z w t" +
				" alternados de operadores matemáticos ( )[ ]{ } ! @ #\n");
			Environment.Exit(0);
		}

		// Verifica se todos os parênteses, colchetes e chaves estão fechados corretamente
		Stack<char> pilha = new Stack<char>(); // cria uma nova instância da classe genérica Stack, o principio é um empilhador de tokes (o utimo que entra é o primeiro que sai), com uma variável "pilha" que será utilizado para comparar se os parenteses, colchetes e chaves foram fechados corretamente e se a algum desses operadores fechados antes de abrir.
		foreach (char cont in entrada) // loop for, utilizado para percorrer cada elemento da variável "entrada"
		{
			if (cont == '(' || cont == '[' || cont == '{') // compara posição a posição se contem um dos três caractere especial presente na lista "CarEspecial".
			{
				pilha.Push(cont); // Caso encontre um parenteses, colchetes e/ou chaves abertas, elas serão empilhadas para comparação.
			}
			else if (cont == ')' || cont == ']' || cont == '}') // Caso encontre um parenteses, colchetes e/ou chaves fechadas, elas serão empilhadas para comparação.
			{
				if (pilha.Count == 0) // caso a variável "pilha" estiver vazia quer dizer que abriu-se um dos 3 operadores e não foi fechado.
				{
					Console.WriteLine("\nExpressão matemática invalida: )  ]  } Não podem vir antes de (  [  {\n"); // imprime a mensagem.
					Environment.Exit(0); // encerra o programa.
				}
				char abertura = pilha.Pop(); // caso encontre algum operador foi aberto e fechado com um outro operador não correspondente Ex: x(y}
				if (cont == ')' && abertura != '(' || cont == ']' && abertura != '[' || cont == '}' && abertura != '{') // compara se o operador qu abriu é o mesmo que fechou.
				{
					Console.WriteLine("\nExpressão matemátca inválida: não foi encontrado um operador de fechamento ) ou ] ou } correspontente\n"); // imprime essa mensagem de erro.
					Environment.Exit(0); //encerra o programa.
				}
			}
		}
		if (pilha.Count != 0) // caso ache um dos 3 operadores abertos e não fechados. ex: x(y+z*w-t
		{
			Console.WriteLine("\nExpressão matemática inválida: (  [  {  devem ser fechados\n"); //imprime esse mensagem
			Environment.Exit(0); //encerra o programa
		}
		else
		{
			Console.WriteLine("\nA string é uma operação matemática válida\n"); // leu todas as sentenças e não achou nenhum erro. Imprime essa mensagem.
		}
	}
}
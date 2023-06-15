// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

namespace RefactoringGuru.DesignPatterns.Builder.Conceptual
{
	// La interfaz Builder especifica métodos para crear las diferentes partes
	// de los objetos Producto.
	public interface IBuilder
	{
		void BuildPartA();

		void BuildPartB();

		void BuildPartC();
	}

	// Las clases Concrete Builder siguen la interfaz Builder y proporcionan
	// implementaciones específicas de los pasos de construcción. El programa puede tener
	// varias variaciones de Constructores, implementadas de forma diferente.
	public class ConcreteBuilder : IBuilder
	{
		private Product _product = new Product();

		// Una nueva instancia de constructor debe contener un objeto producto en blanco, que
		// se utiliza en el montaje posterior..
		public ConcreteBuilder()
		{
			this.Reset();
		}

		public void Reset()
		{
			this._product = new Product();
		}

		// Todos los pasos de producción trabajan con la misma instancia de producto.
		public void BuildPartA()
		{
			this._product.Add("Masa");
		}

		public void BuildPartB()
		{
			this._product.Add("Salsa");
		}

		public void BuildPartC()
		{
			this._product.Add("Queso");
		}

		// Se supone que los Constructores de hormigón deben proporcionar sus propios métodos para
		// recuperar resultados. Esto se debe a que varios tipos de constructores pueden
		// crear productos completamente diferentes que no siguen la misma
		// interfaz. Por lo tanto, tales métodos no pueden ser declarados en la base
		//constructor base (al menos en un lenguaje de programación estáticamente tipado).
		//
		// Normalmente, después de devolver el resultado final al cliente, se espera que una instancia del constructor
		// se espera que esté listo para empezar a producir otro producto.
		// Por eso es una práctica habitual llamar al método reset al final
		// del cuerpo del método `GetProduct`. Sin embargo, este comportamiento no es
		// obligatorio, y puedes hacer que tus constructores esperen una llamada explícita de reset
		// explícita desde el código del cliente antes de deshacerse del resultado anterior.
		public Product GetProduct()
		{
			Product result = this._product;

			this.Reset();

			return result;
		}
	}

	// Tiene sentido utilizar el patrón Builder sólo cuando sus productos son
	// bastante complejos y requieren una configuración extensa.
	//
	// A diferencia de otros patrones de creación, diferentes constructores concretos pueden
	// productos no relacionados. En otras palabras, los resultados de varios constructores
	// pueden no seguir siempre la misma interfaz.
	public class Product
	{
		private List<object> _parts = new List<object>();

		public void Add(string part)
		{
			this._parts.Add(part);
		}

		public string ListParts()
		{
			string str = string.Empty;

			for (int i = 0; i < this._parts.Count; i++)
			{
				str += this._parts[i] + ", ";
			}

			str = str.Remove(str.Length - 2); // removing last ",c"

			return "Product parts: " + str + "\n";
		}
	}

	// El Director sólo es responsable de ejecutar los pasos de construcción en una
	// secuencia determinada. Es útil cuando se fabrican productos según un
	// orden o configuración específicos. Estrictamente hablando, la clase Director es
	// opcional, ya que el cliente puede controlar directamente a los constructores.
	public class Director
	{
		private IBuilder _builder;

		public IBuilder Builder
		{
			set { _builder = value; }
		}

		// El Director puede construir varias variaciones del producto utilizando los mismos
		// pasos de construcción.
		public void BuildMinimalViableProduct()
		{
			this._builder.BuildPartA();
		}

		public void BuildFullFeaturedProduct()
		{
			this._builder.BuildPartA();
			this._builder.BuildPartB();
			this._builder.BuildPartC();
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			// El código del cliente crea un objeto constructor, se lo pasa al
			// director e inicia el proceso de construcción. El resultado
			// final se obtiene del objeto constructor.
			var director = new Director();
			var builder = new ConcreteBuilder();
			director.Builder = builder;

			Console.WriteLine("Standard basic product:");
			director.BuildMinimalViableProduct();
			Console.WriteLine(builder.GetProduct().ListParts());

			Console.WriteLine("Standard full featured product:");
			director.BuildFullFeaturedProduct();
			Console.WriteLine(builder.GetProduct().ListParts());

			// Recuerde, el patrón Constructor puede ser utilizado sin un Director
			// clase.
			Console.WriteLine("Custom product:");
			builder.BuildPartA();
			builder.BuildPartC();
			Console.Write(builder.GetProduct().ListParts());
		}
	}
}


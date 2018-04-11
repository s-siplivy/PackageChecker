using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class FilteringManager
	{
		protected ObservableCollection<string> expressions;

		public FilteringManager(ObservableCollection<string> expressions)
		{
			this.expressions = expressions;
		}

		public void AddExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException("Empty expression.");
			}

			if (!expression.StartsWith("pv:") &&
				!expression.StartsWith("fv:") &&
				!expression.StartsWith("fp:"))
			{
				throw new ArgumentException("Expression should start with «pv:»|«fv:»|«fp:».");
			}

			KeyValuePair<string, string> filter = ParseExpression(expression);
			if (string.IsNullOrEmpty(filter.Value))
			{
				throw new ArgumentException("Filter expression unspecified.");
			}

			expressions.Add(expression);
		}

		public void RemoveExpression(int index)
		{
			if (index < 0)
			{
				throw new ArgumentException("Expression isn't selected.");
			}

			if (index >= expressions.Count)
			{
				throw new ArgumentException("Selection error.");
			}

			expressions.RemoveAt(index);
		}

		protected KeyValuePair<string, string> ParseExpression(string expression)
		{
			string key = expression.Substring(0, 2);
			string value = expression.Substring(3, expression.Length - 3);
			return new KeyValuePair<string, string>(key, value);
		}
	}
}

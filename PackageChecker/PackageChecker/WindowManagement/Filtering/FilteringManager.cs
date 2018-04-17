using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PackageChecker.WindowManagement.Filtering
{
	public class FilteringManager
	{
		protected const string helpMessage =
			"The expression string supports the following format:\n" +
			"\n\t{0}\n\n" +
			"Where:" +
			"\tpv - Product Version property;\n" +
			"\tfv - File Version property;\n" +
			"\tfp - File Path property;\n" +
			"\tsg - Signature property;\n" +
			"\tin - Include operator;\n" +
			"\thl - Highlight operator;\n" +
			"\t* - Value string.\n" +
			"\nThe operator «{1}» inverts boolean value of operators.\n" +
			"It is supported only in the the beginning of the value string.\n" +
			"\nThe operator «{2}» represents any number of symbols.\n" +
			"It is supported in the the beginning and in the end\n" +
			"of the value string.\n" +
			"\nIn case the value string isn't provided it is\n" +
			"considered as an empty string value.\n";

		protected const string hintMessage = "Format is «{0}». For more information, see help.";

		protected const string regExpressionPatternSimplified = "(pv|fv|fp|sg):(in|hl):*";
		protected const string regExpressionPattern = "^(pv|fv|fp|sg){1}:(in|hl){1}:(.*)$";
		Regex regExpression;

		protected ObservableCollection<string> expressions;

		public FilteringManager(ObservableCollection<string> expressions)
		{
			this.expressions = expressions;

			regExpression = new Regex(regExpressionPattern);
		}

		public string GetHelpMessage()
		{
			return string.Format(CultureInfo.InvariantCulture, helpMessage, regExpressionPatternSimplified, FilteringInfo.notSymbol, FilteringInfo.specialSymbol);
		}

		public string GetExpressionPatternHint()
		{
			return string.Format(CultureInfo.InvariantCulture, hintMessage, regExpressionPatternSimplified);
		}

		public FilteringInfo GetFilteringInfo()
		{
			FilteringInfo info = new FilteringInfo();

			foreach (string expression in expressions)
			{
				Tuple<string, string, string> filter = ParseExpression(expression);
				AddExpressionByProperty(info, filter.Item1, filter.Item2, filter.Item3);
			}

			return info;
		}

		public void AddExpression(string expression)
		{
			if (!regExpression.IsMatch(expression))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"An expression should follow the format: {0}", regExpressionPatternSimplified));
			}

			expressions.Add(expression);
		}
		public string EditExpression(int index)
		{
			if (index < 0)
			{
				throw new ArgumentException("Expression isn't selected.");
			}

			if (index >= expressions.Count)
			{
				throw new ArgumentException("Selection error.");
			}

			string expression = expressions[index];
			expressions.RemoveAt(index);

			return expression;
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

		protected Tuple<string, string, string> ParseExpression(string expression)
		{
			Match expressionGroups = regExpression.Match(expression);

			string property = expressionGroups.Groups[1].Value;
			string condition = expressionGroups.Groups[2].Value;
			string value = expressionGroups.Groups[3].Value;
			return new Tuple<string, string, string>(property, condition, value);
		}

		protected void AddExpressionByProperty(FilteringInfo info, string propertyType, string conditionType, string value)
		{
			switch (propertyType)
			{
				case "pv":
					AddExpressionByCondition(info.ProductVersionCondition, conditionType, value);
					break;
				case "fv":
					AddExpressionByCondition(info.FileVersionCondition, conditionType, value);
					break;
				case "fp":
					AddExpressionByCondition(info.FilePathCondition, conditionType, value);
					break;
				case "sg":
					AddExpressionByCondition(info.SignatureCondition, conditionType, value);
					break;
				default:
					throw new InvalidOperationException();
			}
		}

		protected void AddExpressionByCondition(FilteringCondition condition, string conditionType, string value)
		{
			switch (conditionType)
			{
				case "in":
					condition.EntityInclude.Add(value);
					break;
				case "hl":
					condition.EntityHighlignt.Add(value);
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}

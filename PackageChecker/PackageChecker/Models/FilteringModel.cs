using PackageChecker.Filtering;
using PackageChecker.WindowManagement;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PackageChecker.Models
{
	internal class FilteringModel : BindableBase, IFilteringManager
	{
		#region State Implementation
		[Serializable]
		internal class FilteringState
		{
			private ObservableCollection<string> _filteringExpressions;

			private FilteringState(ObservableCollection<string> expressions)
			{
				_filteringExpressions = expressions;
			}

			internal static FilteringState GetState(FilteringModel instance)
			{
				return new FilteringState(instance._filteringExpressions);
			}

			internal void SetState(FilteringModel instance)
			{
				instance._filteringExpressions.Clear();
				foreach (string filter in _filteringExpressions)
				{
					instance._filteringExpressions.Add(filter);
				}
			}
		}
		#endregion //State Implementation

		#region Private Properties
		private int _selectedIndex = -1;
		private string _currentFilteringExpression = string.Empty;
		private string _currentFilteringExpressionHint = string.Empty;
		private ObservableCollection<string> _filteringExpressions = new ObservableCollection<string>();

		private const string _helpMessage =
			"The expression string supports the following format:\n" +
			"\n\t{0}\n\n" +
			"Where:" +
			"\tin - Include operator;\n" +
			"\thl - Highlight operator;\n" +
			"\tpv - Product Version property;\n" +
			"\tfv - File Version property;\n" +
			"\tfp - File Path property;\n" +
			"\tsg - Signature property;\n" +
			"\t* - Value string.\n" +
			"\nThe operator «{1}» inverts boolean value of operators.\n" +
			"It is supported only in the the beginning of the value string.\n" +
			"\nThe operator «{2}» represents any number of symbols.\n" +
			"It is supported in the the beginning and in the end of the value string.\n" +
			"\nIn case the value string isn't provided it is considered as an empty\n" +
			"string value.\n";

		private readonly Regex _regExpression;
		private const string _regExpressionPattern = "^(in|hl){1}:(pv|fv|fp|sg){1}=(.*)$";
		private const string _regExpressionPatternSimplified = "(in|hl):(pv|fv|fp|sg)=*";
		private const string _hintMessage = "Format is «{0}». For more information, see help.";
		#endregion //Private Properties

		#region Binding Properties
		internal readonly ReadOnlyObservableCollection<string> FilteringExpressions;

		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				_selectedIndex = value;
				OnPropertyChanged("SelectedIndex");
			}
		}

		public string CurrentFilteringExpression
		{
			get
			{
				return _currentFilteringExpression;
			}
			set
			{
				_currentFilteringExpression = value;
				OnPropertyChanged("CurrentFilteringExpression");
			}
		}

		public string CurrentFilteringExpressionHint
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, _hintMessage, _regExpressionPatternSimplified);
			}
		}
		#endregion //Binding Properties

		internal FilteringModel()
		{
			FilteringExpressions = new ReadOnlyObservableCollection<string>(_filteringExpressions);

			_regExpression = new Regex(_regExpressionPattern);
		}

		#region Interface implementestion
		public event Action OnFilteringUpdate;

		public FilteringInfo GetFilteringInfo()
		{
			FilteringInfo info = new FilteringInfo();

			foreach (string expression in _filteringExpressions)
			{
				Tuple<string, string, string> filter = ParseExpression(expression);
				AddExpressionByProperty(info, filter.Item1, filter.Item2, filter.Item3);
			}

			return info;
		}
		#endregion //Interface implementestion

		#region Commands Implementation
		internal void AddExpression(string expression)
		{
			if (!_regExpression.IsMatch(expression))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"An expression should follow the format: {0}", _regExpressionPatternSimplified));
			}

			_filteringExpressions.Add(expression);

			OnFilteringUpdate?.Invoke();
		}

		internal void EditExpression(int index)
		{
			if (index < 0)
			{
				throw new ArgumentException("Expression isn't selected.");
			}

			if (index >= _filteringExpressions.Count)
			{
				throw new ArgumentException("Selection error.");
			}

			string expression = _filteringExpressions[index];
			_filteringExpressions.RemoveAt(index);

			CurrentFilteringExpression = expression;

			OnFilteringUpdate?.Invoke();
		}

		internal void RemoveExpression(int index)
		{
			if (index < 0)
			{
				throw new ArgumentException("Expression isn't selected.");
			}

			if (index >= _filteringExpressions.Count)
			{
				throw new ArgumentException("Selection error.");
			}

			_filteringExpressions.RemoveAt(index);

			OnFilteringUpdate?.Invoke();
		}

		internal string GetHelpMessage()
		{
			return string.Format(CultureInfo.InvariantCulture, _helpMessage, _regExpressionPatternSimplified, FilteringInfo.notSymbol, FilteringInfo.specialSymbol);
		}
		#endregion //Commands Implementation

		#region Private Methods
		private Tuple<string, string, string> ParseExpression(string expression)
		{
			Match expressionGroups = _regExpression.Match(expression);

			string @operator = expressionGroups.Groups[1].Value;
			string property = expressionGroups.Groups[2].Value;
			string value = expressionGroups.Groups[3].Value;
			return new Tuple<string, string, string>(property, @operator, value);
		}

		private void AddExpressionByProperty(FilteringInfo info, string propertyType, string conditionType, string value)
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

		private void AddExpressionByCondition(FilteringCondition condition, string conditionType, string value)
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
		#endregion //Private Methods
	}
}

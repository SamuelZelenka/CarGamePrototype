using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/String")]
public class StringCondition : ScriptableObject
{
	
	private const string VALID_MESSAGE = "";
	private const string SHORT_LENGTH_ERROR = "Too short.";
	private const string LONG_LENGTH_ERROR = "Too long.";
	private const string INCLUSION_ERROR = "Does not meet the requirements.";
	private const string EXCLUSION_ERROR = "Is not allowed.";

	[SerializeField]
	private bool _include;

	[SerializeField]
	private string[] strings;

	[SerializeField]
	private int _minLength;

	[SerializeField]
	[ShowIf("_useMaxLength")]
	private int _maxLength;

	[SerializeField]
	private bool _useMaxLength;

	public bool IsValid(string input, out string message)
	{
		bool isTooLong = _useMaxLength && input.Length - 1 > _maxLength;
		bool isTooShort = input.Length - 1 < _minLength;
		bool contentValid = _include ? strings.Contains(input) : !strings.Contains(input);

		if (!isTooLong && !isTooShort && contentValid)
		{
			message = VALID_MESSAGE;
			return true;
		}

		if (isTooLong || isTooShort)
		{
			message = isTooShort ? SHORT_LENGTH_ERROR : LONG_LENGTH_ERROR;
		}
		else
		{
			message = _include ? INCLUSION_ERROR : EXCLUSION_ERROR;
		}

		return false;
	}
}
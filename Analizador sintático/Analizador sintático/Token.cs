using System;
public enum TokenTypes
{
	action,
	ally,
	ask,
	condition,
	enemy,
	objects,
	separator,
	delimiter,
	error,
	open_parentesis,
	close_parentesis,
	self,
	number,
}
public class Token
{
	private Tuple<int,int> position;
	private string value;
	private TokenTypes type;
	private string scope;
	private int dummyPointer;
	public Token(Tuple<int,int> _position, string _value, TokenTypes _type)
	{
		this.position = _position;
		this.value = _value;
		this.type = _type;
		//this.scope = _scope;
		//this.dummyPointer = _dummyPointer;
		//string _scope, int _dummyPointer
	}
	public string getScope()
    {
		return this.scope;
    }
	public void setScope(string _scope)
	{
		this.scope = _scope;
	}

	private string formatString()
    {
		return "<lexema: '" + this.value + "', tipo do lexema: " + this.type + ">";
	}

	public override string ToString()
	{
		return formatString();
	}
}

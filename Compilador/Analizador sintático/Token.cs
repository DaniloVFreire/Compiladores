using System;
public enum TokenTypes
{
	action, //ID
	ally, // ID
	ask, //ID
	condition, //ID
	enemy, // ID
	objects, // ID
	separator,
	delimiter,
	error, // ID
	open_parentesis,
	close_parentesis,
	self, // ID
	number,
	endLine,
}
public class Token
{
	private Tuple<int, int> position;
	private string value;
	private TokenTypes type;
	private string scope;
	public Token(Tuple<int, int> _position, string _value, TokenTypes _type)
	{
		this.position = _position;
		this.value = _value;
		this.type = _type;
	}
	public string getScope()
	{
		return this.scope;
	}
	public void setScope(string _scope)
	{
		this.scope = _scope;
	}

	public void setPosition(Tuple<int, int> _position)
    {
		this.position = _position;

	}

	public string getPosition()
	{
		return this.position.ToString();
	}
	public Tuple<int, int> getPositionInt()
	{
		return this.position;
	}
	public string getType()
	{
		return this.type.ToString();
	}
	public string getValue()
    {
		return this.value;
    }

	private string formatString()
	{
		return $"<posição: {this.position}, lexema: \"{this.value}\", tipo do lexema: { this.type} >";
	}

	public override string ToString()
	{
		return formatString();
	}
}
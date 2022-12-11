using System;

public class TestGenerator
{
	public TestGenerator()
	{
	}

	public void FS() {
		String prefix = "", sufix = "";
		FSequence(prefix, sufix);
		FAsk(prefix, sufix);
	}
	private void FAsk(string prefix, string sufix)
	{
		sufix += " ( allyNA0 ) .";
		FAskAction(prefix, sufix);
		FAskInfo(prefix, sufix);
	}
	private void FAskInfo(string prefix, string sufix)
	{
		prefix += "askAction ";
		sufix += " ( allyNA0 ) .";
		FCondition(prefix, sufix);
	}
	private void FAskAction(string prefix, string sufix)
	{
		prefix += "askAction ";
		sufix += " ( allyNA0 ) .";
		FAction(prefix, sufix);
	}

	private void FSequence(string prefix, string sufix)
    {
		sufix += " .";
		FAction(prefix, sufix);
		FCondition(prefix, sufix);
    }

	private void FCondition(string prefix, string sufix)
	{
		FCarryingBall(prefix, sufix);
		FMarked(prefix, sufix);
		FPosition(prefix, sufix);
		FNeighbors(prefix, sufix);
	}

	private void FCarryingBall(string prefix, string sufix)
	{
		prefix += "carryingBall ( ";
		sufix = " )" + sufix;
		FDObjects(prefix, sufix);
	}
	private void FPosition(string prefix, string sufix)
	{
		prefix += "position ( ";
		sufix = " )" + sufix;
		FDObjects(prefix, sufix);
	}
	private void FNeighbors(string prefix, string sufix)
	{
		prefix += "neighbors ( ";
		sufix = " )" + sufix;
		FObjects(prefix, sufix);
	}
	private void FMarked(string prefix, string sufix)
	{
		prefix += "marked ( ";
		sufix = " )" + sufix;
		FDObjects(prefix, sufix);
	}

	private void FAction(string prefix, string sufix) {
		FMoveTowards(prefix, sufix);
		FExplore(prefix, sufix);
		FSendBall(prefix, sufix);
		FSayOk(prefix, sufix);
		FSayNo(prefix, sufix);
		FSayPosition(prefix, sufix);
		FHelp(prefix, sufix);
	}
	private void FExplore(string prefix, string sufix)
	{
		Console.WriteLine(prefix + "explore" + sufix);
	}
	private void FSendBall(string prefix, string sufix)
	{
		prefix += "sendBall ( ";
		sufix = " )" + sufix;
		FAlly(prefix,sufix);
		FEnemyGoal(prefix, sufix);
		FSelf(prefix, sufix);
	}
	private void FSayOk(string prefix, string sufix)
	{
		prefix += "sayOk ( ";
		sufix = " )" + sufix;
		FAlly(prefix, sufix);
	}
	private void FSayNo(string prefix, string sufix)
	{
		prefix += "sayNo ( ";
		sufix = " )" + sufix;
		FAlly(prefix, sufix);
	}

	private void FSayPosition(string prefix, string sufix)
	{
		prefix += "sayPosition ( ";
		sufix = ", 0 , 0 )" + sufix;
		FObjects(prefix, sufix);
	}

	private void FObjects(string prefix, string sufix)
	{
		FDObjects(prefix, sufix);
		FSObjects(prefix, sufix);
	}

	private void FHelp(string prefix, string sufix)
	{
		prefix += "help ( ";
		sufix = " )" + sufix;
		FObjects(prefix, sufix);
	}

	private void FMoveTowards(string prefix, string sufix)
	{
		prefix += "moveTowards ( ";
		sufix = " )" + sufix;
		FDObjects(prefix, sufix);
		FSObjects(prefix, sufix);
	}
	private void FSObjects(string prefix, string sufix)
	{
		FEnemyGoal(prefix, sufix);
		Console.WriteLine(prefix + "allyGoal" + sufix);
	}
	private void FEnemyGoal(string prefix, string sufix)
	{
		Console.WriteLine(prefix + "enemyGoal" + sufix);
	}
	private void FDObjects(string prefix, string sufix)
    {
		FEnemy(prefix, sufix);
		FAlly(prefix, sufix);
		FSelf(prefix, sufix);
	}
	private void FEnemy(string prefix, string sufix)
	{
		Console.WriteLine(prefix + "enemyNA0" + sufix);
	}
	private void FAlly(string prefix, string sufix)
	{
		Console.WriteLine(prefix + "allyNA0" + sufix);
	}
	private void FSelf(string prefix, string sufix)
	{
		Console.WriteLine(prefix + "self" + sufix);
	}
}

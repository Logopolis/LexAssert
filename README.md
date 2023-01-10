# LexAssert
LexAssert extends Assert

## Motivaton
A common difficulty with unit test writing is asserting equality between an object generated by code under test 
and an object with expected set of values. 

LexAssert provides ways of expressing that objects are "equal enough" for the purposes of an xUnit test.

## Public objects
**Lassert** exposes some new asserts for use with xUnit.
**JsonEqualityComparer** can be used to compare Json serializations of an object

## Public methods of Lassert
**void JsonEqual<T>(T expected, T actual)** passes if expected and actual yield identical strings when serialized in Json. 
Throws an EqualException if the serialized strings are not equal.

Examples:
```cs
internal class MyTestClass
{
    string PropA => "foo";
    int PropB => 63;
}

[Fact]
public void JsonEqual_Demo1()
{
    var actual = new MyTestClass();
    var expected = new MyTestClass();

    Lassert.JsonEqual(expected, actual); // Passes
}

[Fact]
public void JsonEqual_Demo2()
{
	var expected = new Dictionary<string, object>
	{
		{ "PropA", "foo" },
		{ "PropB", 63 }
	};

	var actual = new MyTestClass();

	Lassert.JsonEqual(expected, actual); // Passes, as both objects yield identical Json.
}
```

**public static void MembersEqual<T>(T expected, T actual, params Func<T, object>[] memberSelectors)** passes if the selector functions,
when applied to each of the inputs, yield equal values in each case. 
Throws an EqualException if the values are different.

This can be used to specify which members of a type are important when comparing.

Example:
```cs
internal class AnotherTestClass
{
    public string PropA { get; set; }
    public int PropB { get; set; }
    public System.Guid PropC { get; set; }
}

[Fact]
public void MembersEqual_Demo1()
{
    var actual = new AnotherTestClass
    {
        PropA = "foo",
        PropB = 63,
        PropC = System.Guid.NewGuid()
    };

    var expected = new AnotherTestClass
    {
        PropA = "foo",
        PropB = 63,
        PropC = System.Guid.NewGuid()
    };

    Lassert.MembersEqual(expected, actual,
        c => c.PropA,
        c => c.PropB);  // Passes, because PropC is not compared.
}
```

The functions comparing the two objects can be more complicated if needed:
```cs
[Fact]
public void MembersEqual_Demo2()
{
    var actual = "foo";
    var expected = "bar";

    Lassert.MembersEqual(expected, actual,
        c => c.ToLowerInvariant().Contains("z")); // Passes, because false == false.
}
```

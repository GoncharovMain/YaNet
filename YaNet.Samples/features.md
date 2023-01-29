

```
valid("a: v1\n", "scalar")

a: v1
```

```
valid("- item", "item")

- item
```

```
valid("a:\n\tb: v1\n", "node")

a:
	b: v1
```

```
valid("a: ${ref}\n\tb: v1\n", "node ref")

a: ${ref}
	b: v1
```

```
valid("a:\n\t- b\n", "set")

a:
	- b
```

```
valid("a: ${ref}\n\t- b\n", "set ref")

a: ${ref}
	- b
```

```
valid("a:\n\t- b: v1\n", "set")

a:
	- b: v1
```

```
valid("- a:\n\tb: v1\n", "node of item")

- a:
	b: v1
```

```
valid("\t- a\nb:v1\n", "item")

	- a
b: v1
```
Simple node: item or scalar.
```
valid("\t- a: v1\nb:v2\n", "node of item")

	- a: v1
b: v2
```

INode
Node `a: v1`

ItemScalar `- a`
KeyScalar `a: v1`
Scalar `a: v1`

ItemKeyScalar `- a: v1`
ItemKeyScalar `- a: v1`


```
Node

Scalar
	Value

Pair : Node
	Scalar Key
	Scalar Value

Item : Node
	Node (Pair, Scalar)

```


`key: value` Pair
`key: reference` Pair
`key:` Node



Item.Scalar
`- item` Item
`- scalar` Scalar
`- key: value` Item
`- node:`
`- node: reference`

`node:`
`node: reference`

---

```
public interface INode
{
	void InitProperty();
}

public class Node : INode
{
	public INode List : Node { get; set; }
	public INode Dictionary : Node { get; set; }
	public INode Object : Node { get; set; }
}

public class Pair : INode
{
	public Mark Key { get; set; }
	public Mark Value { get; set; }
}

public class Item : Node
{
	public INode Data { get; set; }
}

public class Scalar : Node
{
	public Mark Value { get; set; }
}

public class Collection : Node
{
	public Scalar[] Values { get; set; }
	public Pair[] KeyValues { get; set; }
}

```


```
KeyValue (scalar: scalar)

a: v1
```

```
Item
	Value (scalar)

- a
```

```
a:
Node
	Pair[], Item[] : Node

```

```
Node
	KeyValue[] (collection of KeyValue)

- a: v1
  b: v2
  c: v3

```

```

```

asd
Node.Collection<Items>.KeyValue[]
- a: v1
  b: v2
  c: v3
- a: v4
  b: v5
  c: v6

```

```

key: value
key: reference
key:
- item
- scalar
- key: value
- node:
- node: reference

node:
node: reference

```



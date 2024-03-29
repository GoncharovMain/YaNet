<h1>Documentation</h1>

**Features:**

The Clanguage has static typing, and the data types in the `yanet` configuration file will depend on the data types of the object's properties.

```
person:
  name: John
  age: 18
```

<p>First case value 18 will be convert to <b>string</b> type.</p>

```
class Person
{
  public string Name { get; set; }
  public string Age { get; set; }
}
```
<p>Second case value 18 will be convert to <b>int</b> type</p>

```
class Person
{
 public string Name { get; set; }
 public int Age { get; set; }
}
```

в отличии от формата yanet, в json тип определяется явно через, например,
двойные ковычки:
"18" является типом string
18 является числовым типом

Хотя в yanet существует явное выделение значения двойными ковычками
```
person:
  name: "John"
  age: "18"
```

и при попытке преобразовать в тип int, YaNet выдаст исключение

*Чем отличается синтаксис тегов от нового синтаксиса ссылок?*

Вместо объявления новой переменной можно использовать каскад ссылок,
например: `requests.google.search.response.expectedField`. Всё заключается между
двумя последовательностями знаков: '${' и '}'

Время инициализации ссылок

Ссылки заменяют не в момент парсинга, а в момент обращения к ним.
Ссылка имеет возможность ссылаться на поле, которое ссылается на другое поле

```
text: scalar

expectedField: ${text}

result: ${expectedField}
```

Если всё же хочется определить переменную, то просто добавьте поле
и задайте ей значение. Возможен случай, когда значение поля надо брать не из
области видимости текущей разметки, а из другого файла, тогда можно импортировать
файл указав словарь import.

Импорт файлов

Поле import зарезервировано и используется в самом начале. Создаётся словарь
в формате <название_объекта>:<путь_к_файлу>, где ключ это строка, а объект это весь документ.

```
google.yaml file

request:
  header:
  cookie: 9ghfd93fh

yandex.yaml

request:
  header:
  cookie: 3h4j4298f

request.yaml

import:
  google: folder1/folder/google.yaml
  yandex: folder1/folder/yandex.yaml

cookies:
  - ${google.request.header.cookie}
  - ${yandex.request.header.cookie}
```

result: cookies => { "9ghfd93fh", "3h4j4298f" }

with indent 2 spaces

```
LF
list:/n  - item1/n  - item2/n  - item3/n  - itemN/n

CR
list:/r  - item1/r  - item2/r  - item3/r  - itemN/r

EOL
list:/r/n  - item1/r/n  - item2/r/n  - item3/r/n  - itemN/r/n
```



with tab

```
LF
list:/n/t- item1/n/t- item2/n/t- item3/n/t- itemN/n

CR
list:/r/t- item1/r/t- item2/r/t- item3/r/t- itemN/r

EOL
list:/r/n/t- item1/r/n/t- item2/r/n/t- item3/r/n/t- itemN/r/n
```

Почему стоит добавить новый синтаксис ссылок?

```
person:
  name: John
  age: 18
```


YaNet version 1.0:
  - simple reference

YaNet version 2.0
  - Settings:
  - using '\t' or double space ' '
  - replace delimiter ':' on "=>"

YaNet version 3.0
  -


T => scalar, list, dictionary, object

tag: <scalar>
- значит, что ссылок может быть сколько угодно и в любом месте
ссылки внутри других ссылок не поддерживаются

```
text: name
baz: bar
bar: baz
  name: hello
foo: ${baz.{text}}
```
или
```
foo: ${baz.${text}}
ожидается => ${bar.name} => hello
```
но это не работает.

```
text: scalar [${ref}*]
text: 'scalar'
text: "scalar"
```
```
list: <scalar, T>

list: ${ref}
  - item1 [${ref}*]
  - item2 [${ref}*]
  - item3 [${ref}*]
  ...
  - itemN [${ref}*]

dictionary => (key:<scalar>): (value: <T>)
```

```
dictionary: ${ref}
  key1: value1 [${ref}*]
  key2: value2 [${ref}*]
  key3: value3 [${ref}*]
  ...
  keyN: valueN [${ref}*]

{
  { "key1", "value1" },
  { "key2", "value2" },
  { "key3", "value3" },
  ...
  { "keyN", "valueN" },
}

object:
  prop1: value1
  prop2: value2
  prop3: value3
  ...
  propN: valueN

new Object { Prop1 = "value1", Prop2 = "value2", Prop3 = "value3", ..., PropN = "valueN" }

```


<h2>Replacement reference</h2>



**scalar to scalar**

```
text: scalar
foo: ${text}

result: foo => "scalar"
```


**scalar to partial scalar**
```

text: scalar
foo: hello ${text}

result: foo => "hello scalar"
```
```
name: Bob
foo: Hello, ${name}. How are you?

result: foo => "Hello, Bob. How are you?"
```

**scalar to partial several scalar**
```
firstName: Bob
secondName: Ali
foo: Hello, ${firstName} ${secondName}. How are you?

result: foo => "Hello Bob Ali. How are you?"
```



**Exception recursion**

```
foo: hello, ${bar}
bar: hello, ${foo}

result: new Exception("Recursion, foo and bar have reference on yourself")
```

**scalar to one item of list**

```
name: Sara
names:
  - John
  - Sam
  - Bob
  - ${name}
  - Nick
  - Nelson

result: names => { John, Sam, Bob, Sara, Nick, Nelson }
```



**scalar to several items of list**

```
name: Sara
name1: Bob
names:
  - John
  - Sam
  - ${name1}
  - ${name}
  - Nick
  - Nelson
  - ${name1}

result: names => { "John", "Sam", "Bob", "Sara", "Nick", "Nelson", "Bob" }
```

**get scalar on yourself items**

```
names:
  - John
  - Sam
  - Bob
  - Sara
  - Nick
  - Nelson
  - ${names[2]}

result: names => { "John", "Sam", "Bob", "Sara", "Nick", "Nelson", "Bob" }
```


**scalar from list to item of list**

```
names1:
  - John 0
  - Sam  1
  - Bob  2

names2:
  - ${names1[2]}
  - Nick
  - Nelson

result: names2 => { "Bob", "Nick", "Nelson" }
```



**Override list**

```
names1:
  - John
  - Sam
  - Bob
names2: ${names1}

result: names2 => { "John", "Sam", "Bob" }
```



**Add items to end to list**
```
names1:
  - John
  - Sam
  - Bob
names2: ${names1}
  - Sara
  - Nick
  - Nelson

result: names => { "John", "Sam", "Bob", "Sara", "Nick", "Nelson", "Bob" }
```
**Exception: index out of range **
```
names1:
  - John 0
  - Sam  1
  - Bob  2

names2:
  - ${names1[3]}
  - Nick
  - Nelson

result: new Exception("Index out of range.")
```

**Exception yourself**
```
names:
  - ${names[0]}
  - Sam
  - Bob

result: new RecursionException("Index have reference on youself.")
```


**Scalar to item of dictionary**

```
name: Bob
person:
  name: ${name}
  age: 18

result: person => { { "name", "Bob" }, { "age", "18" } }

name: Bob
age: 18
person:
  name: ${name}
  age: ${age}

result: person => { { "name", "Bob" }, { "age", "18" } }
```


**Scalar to composite field**

```
firstName: Bob
secondName: Ali
age: 18
person:
  fullName: ${firstName} ${secondName}
  age: ${age}

result: person => { { "fullName", "Bob Ali" }, { "age", "18" } }
```

**Scalar from yourself field**
```
person:
  firstName: Bob
  secondName: Ali
  fullName: ${person.firstName} ${person.secondName}
  age: 18

result: person => { { "firstName", "Bob" }, { "secondName", "Ali" },
{ "fullName", "Bob Ali" }, { "age", "18" } }
```


<h2>Objects</h2>

**Scalar to property of object**

```
name: Bob
person:
  name: ${name}

result: person => Person { Name = "Bob" }
```

```
name: Bob
age: 18
person:
  name: ${name}
  age: ${age}

result: person => Person { Name = "Bob", Age = 18 }
```



**Scalar from object property to other object property**
*Types of property Person and Person2 IS NOT equal*.

```
person:
  name: Bob
  age: 18

person2:
  name: ${person.name}
  age: ${person.age}

result: person2 => Person2 { Name = "Bob", Age = 18 }
```



**Override full object**
*Types of property Person and Person2 IS equal*.

```
person:
  name: Bob
  age: 18

person2: ${person}

result: person2 => Person { Name = "Bob", Age = 18 }
```



**Override partial object**
```
person:
  name: Bob
  age: 18

person2: ${person}
  age: 20

result: person2 => Person { Name = "Bob", Age = 20 }
```
**Exception: conflict references**



**Delimiters**

**delimiter for dictionary**
```
person:
  name: Bob ": "
  age: 18   ": "
```

**delimiter for object**
```
person:
  name => Bob " => "
  age => 18   " => "
```

<h2>TOKENS</h2>

**Scalar => string, int, char, double and other type struct**

```
text: Scalar [${ref}*]


tokens: { "text", "scalar" }
key: text
value: Scalar
type: Scalar
```


```
list: ${ref}
  - Scalar
  - Scalar
  - Scalar

tokens: { "list", "item1", "item2", "item3", ..., "itemN" }
key: list
value: { "item1", "item2", "item3", ..., "itemN" }
type: List<string>
refOverride: ref
```

```
dictionary: ${ref}
  key1: Scalar
  key2: Scalar
  key3: Scalar
```

```
tokens: { "dictionary", "key1: value1", "key2: value2", "key3: value3", ..., "keyN: valueN" }
key: dictionary
value: { "key1: value1", "key2: value2", "key3: value3", ..., "keyN: valueN" }
type: Dictionary<string, string>
refOverride: ref
```

```
object:
  prop1: value1
  prop2: value2
  prop3: value3
  ...
  propN: valueN
```

```
string[] tokens = new string[] { "text", "scalar"}
int indent = 0;
Reference[] reference = new Reference[];
```
```
type: list
refOverride: ref
token_key: [ key1, key2, key3 ]
token_value: [ value1, value2, value3 ]
```

```
type: dictionary
refOverride: ref
token_key: [ 0, 1, 2 ]
token_value: [ item1, item2, item3 ]
```

```
token_key: text
token_value: Hello, ${name}. Are you ${age}?
type: Scalar
refs:
  - ${name}
  - ${age}
```

**start mark**


**new syntaxis???**
```
names:
  1. Bob
  2. John
  3. Ali

name: Bob
```


```
LineBreak:
  - LF
  - CRLF
```

*if first symbol after token-key is '@' then ignore reference*
*for (*) -> token-key*

scalar

token-types:
  - token-scalar
  - token-list
  - token-dictionary
  - token-object
  - token-key
  - token-property
  - token-reference

[token]: [token]



token: `${text} -> (token-scalar, token-list, token-dictionary or token-object)`
token: `this is ${text} text -> (token-scalar)`


token-key: token-scalar [partial-scalar]* (1)

text: this is text


list

If second line start substring is `\t- ` then token-key have token-list type
The starting position of the substring is determined by the LevelIndent

[token]:\n{\t- [token]\n}{\t- [token]\n}{\t- [token]}

[token]:
  - [token]
  - [token]
  - [token]

token-key: token-list [reference] (2)
  - token-scalar [partial-scalar]*
  - token-scalar [partial-scalar]*
  - token-scalar [partial-scalar]*

names:
  - John
  - Bob
  - Patrik


dictionary

[token]:
  [token]: [token]
  [token]: [token]
  [token]: [token]

token-key: token-dictionary [reference] (3)
  token-key: token-scalar [partial-scalar]*
  token-key: token-scalar [partial-scalar]*
  token-key: token-scalar [partial-scalar]*

phones:
  John: 88005552524
  Bob: 88005552525
  Patrik: 88005552526



token-scalar extended token
token-key extended of token-scalar

token-scalar, token-list and token-dictionary follow token-key



What is type of token?
When parsing, data should be presented in 1 dimensional form.


Scalar

delimiter:
`: ` -> (token-scalar)


List

token:
  - []
-> (token-list)

token: ${token-ref}
  - []
-> (token-list)


regular expression for LF and CRLF format
delimiter:

`:\n(\t)*- ` -> (token-list)
`: ${token-ref}\n(\t)*- ` -> (token-list) (override)

`:\n(\t)*- ` -> (token-list)
`: ${token-ref}\n(\t)*- ` -> (token-list) (override)

(\t)* - count tab indent is LevelIndent



token:
  - token
  - token
  - token
  - token

token:\n\t- token\n\t- token\n\t- token\n\t- token
token:\n\t- token\n\t- token\n\t- token\n\t- token

`\n\t- `{token}`\n\t- `{token}`\n\t- `{token}`\n\t- `{token}
`\t- `{token}`\n\t- `{token}`\n\t- `{token}`\n\t- `{token}

{token}`\n\t- `{token}`\n\t- `{token}`\n\t- `{token}

delimiter between items of list for LF and CRLF

`\n\t- `
`\n\t- `


dictonary or object

token:
  token: token

regular expression for LF and CRLF format
delimiter:

`:\n(\t)*` -> (token-object, token-dictionary)
`: ${token-reference}\n(\t)*` -> (token-object, token-dictionary) (override)


`:\n(\t)*` -> (token-object, token-dictionary)
`:\n${token-reference}(\t)*` -> (token-object, token-dictionary) (override)


delimiter between items of object property or items of dictionary

token:
  token: token
  token: token
  token: token
  token: token

`token:\n\ttoken: token\n\ttoken: token\n\ttoken: token\n\ttoken: token
    \n\ttoken: token\n\ttoken: token\n\ttoken: token\n\ttoken: token
      token: token\n\ttoken: token\n\ttoken: token\n\ttoken: token`

delete first token (key) and delimiter for LF and CRLF

{token: token}`\n\t`{token: token}`\n\t`{token: token}`\n\t`{token: token}
{token: token}`\n\t`{token: token}`\n\t`{token: token}`\n\t`{token: token}

delimiter:
`\n(\t)*`
`\n(\t)*`


token:
  token:
  - token: token
    token: token
    token: token


`token:\n\ttoken:\n\t\t- token: token\n\t\t\ttoken: token\n\t\t\ttoken: token
      token:\n\t\t- token: token\n\t\t\ttoken: token\n\t\t\ttoken: token
          \t\t- token: token\n\t\t\ttoken: token\n\t\t\ttoken: token
            token: token\n\t\t\ttoken: token\n\t\t\ttoken: token`


\n - delimiter
\t - count tab is level indent

Exceptions:
  SyntaxException:
  - IndentException
  - DelimiterException
  - ReferenceException

  TypeException:
  - Int32Exception


`person:\n\tpersonal data:\n\t\tfirstName: Bob\n\t\tmiddleName: John\n\t\tsecondName: Patick\n\t\tage: 18\n\t\tsex: male\n\taddress:\n\t\tcity: Moscow\n\t\tstreet: Red\n\t\thome: 3\n\tvisitCountries:\n\t\t- Russia\n\t\t- China\n\t\t- USA\n\tfriends:`
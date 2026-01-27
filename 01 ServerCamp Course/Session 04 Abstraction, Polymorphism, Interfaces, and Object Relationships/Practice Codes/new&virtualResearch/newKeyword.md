# Question:
```
class Base
{
    public void Show()
    {
        Console.WriteLine("Base");
    }
}

class Derived : Base
{
    public new void Show()
    {
        Console.WriteLine("Derived");
    }
}
```
what will be the output?
# Answer:
Classic trick question 😄
Let’s be precise and then I’ll explain **why**.

---

## Code recap

```csharp
Base b1 = new Base();
b1.Show();

Base b2 = new Derived();
b2.Show();

Derived d = new Derived();
d.Show();
```

---

## Output

```
Base
Base
Derived
```

---

## Why this happens (the key idea)

### `new` ≠ polymorphism

This line in `Derived`:

```csharp
public new void Show()
```

does **method hiding**, **not overriding**.

That means:

* The method call is resolved **at compile time**
* Based on the **reference type**, not the object type

---

## Case-by-case breakdown

### 1️⃣

```csharp
Base b1 = new Base();
b1.Show();
```

* Reference type: `Base`
* Object type: `Base`
* Calls `Base.Show()`

✅ Output:

```
Base
```

---

### 2️⃣ (the trap)

```csharp
Base b2 = new Derived();
b2.Show();
```

* Reference type: `Base`
* Object type: `Derived`
* Because `Show()` is **not virtual**, and `new` hides it,
  C# uses the **reference type**

So:

```csharp
Base.Show()
```

✅ Output:

```
Base
```

---

### 3️⃣

```csharp
Derived d = new Derived();
d.Show();
```

* Reference type: `Derived`
* Calls `Derived.Show()`

✅ Output:

```
Derived
```

---

## Why this is dangerous (interview insight ⚠️)

This code **looks polymorphic but isn’t**.

Many devs *expect*:

```
Base
Derived
Derived
```

But that only happens with `virtual` + `override`.

---

## How to fix it (real polymorphism)

### Correct version:

```csharp
class Base
{
    public virtual void Show()
    {
        Console.WriteLine("Base");
    }
}

class Derived : Base
{
    public override void Show()
    {
        Console.WriteLine("Derived");
    }
}
```

Now:

```csharp
Base b2 = new Derived();
b2.Show();
```

✅ Output:

```
Derived
```

Because:

* Call is resolved at **runtime**
* Based on **object type**

---

## One-line interview takeaway

> `new` hides a method and uses compile-time binding, while `override` enables polymorphism and uses runtime binding.

If you want, next I can:

* Explain **method hiding vs overriding** with memory diagrams
* Show why `new` even exists
* Give you **interview one-liners** for this question

Just say the word 👌

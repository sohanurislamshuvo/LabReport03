# Lab Report 03 — Abstract Classes & Inheritance in C#

A small console program that models geometric shapes to demonstrate four core
object-oriented concepts: **abstraction**, **inheritance**, **method overriding**,
and **polymorphism**.

---

## Program Output

```
This shape is a: Circle
Area: 78.54

This shape is a: Rectangle
Area: 24.00
```

---

## Line-by-Line Walkthrough

### The `using` directive and namespace

```csharp
using System;
```

**Line 1** — Imports the `System` namespace. This is what lets us write `Console.WriteLine(...)`
and `Math.PI` instead of the full names `System.Console.WriteLine(...)` and `System.Math.PI`.

```csharp
namespace LabReport03
{
```

**Line 3** — Declares a namespace, a named container for our classes. It prevents naming
collisions: our `Rectangle` class won't clash with any other `Rectangle` defined elsewhere
(for example, `System.Drawing.Rectangle`).

---

### The abstract base class: `Shape`

```csharp
abstract class Shape
{
```

**Line 5** — Declares `Shape` as an **abstract class**. The `abstract` keyword means:

- You **cannot** create an object of it directly — `new Shape()` is a compile-time error.
- It exists only to be inherited from. It defines *what every shape must be able to do*,
  without saying *how*.

This makes sense conceptually: "circle" and "rectangle" are real things you can draw,
but a generic "shape" is just an idea.

```csharp
public abstract double Area();
```

**Line 7** — An **abstract method**. Notice it has no body — it ends with a semicolon instead
of `{ }`. This is a *contract*: it declares that every shape has an area, but refuses to
guess how to calculate it, because the formula differs for every shape.

Any non-abstract class inheriting from `Shape` is **forced by the compiler** to supply its
own `Area()`. Forget it, and the code won't build.

```csharp
public void DisplayShape()
{
    Console.WriteLine("This shape is a: " + this.GetType().Name);
}
```

**Lines 9–12** — A **concrete method** (it has a real body). Unlike `Area()`, this method
*is* shared: every shape displays its name the same way, so the logic is written once here
and inherited by all subclasses.

`this.GetType()` inspects the object's **actual runtime type**, not the type of the variable
holding it. `.Name` pulls out the short class name as a string. So when a `Circle` calls this
method, it prints `"Circle"` — even though the code lives in `Shape` and `Shape` has no idea
its subclasses exist. This is **runtime polymorphism** in action.

---

### First derived class: `Circle`

```csharp
class Circle : Shape
{
```

**Line 15** — The colon `:` means **"inherits from"**. `Circle` is a `Shape`, so it
automatically receives the `DisplayShape()` method and inherits the *obligation* to
implement `Area()`.

```csharp
private double radius;
```

**Line 17** — A **private field** storing the circle's radius. `private` means it's
accessible only inside `Circle` — no outside code can read or corrupt it. This is
**encapsulation**: the data is sealed inside the object that owns it.

```csharp
public Circle(double radius)
{
    this.radius = radius;
}
```

**Lines 19–22** — The **constructor**, which runs automatically when you write `new Circle(5)`.
It takes a radius and stores it in the field.

The parameter is named `radius` and so is the field, which creates a shadow. `this.radius`
explicitly means *"the field belonging to this object"*, while plain `radius` refers to the
parameter. So `this.radius = radius;` reads as **"assign the incoming value to my field."**
Without `this.`, the line would assign the parameter to itself and the field would stay `0`.

```csharp
public override double Area()
{
    return Math.PI * radius * radius;
}
```

**Lines 24–27** — Fulfills the contract. The **`override` keyword** tells the compiler this
method deliberately replaces the abstract `Area()` inherited from `Shape`.

The body implements the formula **A = πr²**, using `Math.PI` (≈ 3.14159265358979) for accuracy
rather than a hard-coded `3.14`.

---

### Second derived class: `Rectangle`

```csharp
class Rectangle : Shape
{
    private double length;
    private double width;
```

**Lines 30–33** — Same inheritance pattern, but `Rectangle` needs **two** measurements
instead of one. This is exactly why `Area()` had to be abstract: the two shapes don't even
store the same *kind* of data, so no single shared formula could work.

```csharp
public Rectangle(double length, double width)
{
    this.length = length;
    this.width = width;
}
```

**Lines 35–39** — A constructor taking two arguments, using `this.` for the same
disambiguation reason as `Circle`.

```csharp
public override double Area()
{
    return length * width;
}
```

**Lines 41–44** — The rectangle's own implementation: **A = l × w**. Two classes, one method
name, completely different formulas — that is **method overriding**.

---

### The entry point: `Program.Main`

```csharp
class Program
{
    static void Main()
    {
```

**Lines 47–50** — `Main` is the **entry point**: execution begins here when the program runs.
It is `static`, meaning it belongs to the class itself rather than to an instance, so the
runtime can call it without first constructing a `Program` object.

```csharp
Circle circle = new Circle(5);
circle.DisplayShape();
Console.WriteLine("Area: " + circle.Area().ToString("F2"));
```

**Lines 51–53** — Three steps:

1. `new Circle(5)` allocates a circle and runs its constructor, storing `radius = 5`.
2. `circle.DisplayShape()` calls the **inherited** method — code that `Circle` never wrote —
   and it correctly prints `This shape is a: Circle`.
3. `circle.Area()` computes `π × 5 × 5 = 78.5398...`, and `.ToString("F2")` formats it as
   **F**ixed-point with **2** decimal places, giving `78.54`.

```csharp
Console.WriteLine();
```

**Line 55** — Prints an empty line to visually separate the two shapes in the output.

```csharp
Rectangle rectangle = new Rectangle(4, 6);
rectangle.DisplayShape();
Console.WriteLine("Area: " + rectangle.Area().ToString("F2"));
```

**Lines 57–59** — The identical three steps for a rectangle: `4 × 6 = 24`, formatted as `24.00`.

Note that the *calling code is structurally identical* for both shapes, even though the
underlying area calculations are entirely different. The correct `Area()` is selected
automatically at runtime based on the object's real type. That is the payoff of polymorphism.

---

## Concept Summary

| Concept | Where it appears | What it accomplishes |
|---|---|---|
| **Abstraction** | `abstract class Shape` | Defines the idea of a shape without implementing one |
| **Abstract method** | `public abstract double Area();` | Forces every subclass to define its own area formula |
| **Inheritance** | `class Circle : Shape` | Subclasses reuse `DisplayShape()` for free |
| **Method overriding** | `public override double Area()` | One method name, per-shape behavior |
| **Polymorphism** | `this.GetType().Name` | Base-class code resolves the real subclass at runtime |
| **Encapsulation** | `private double radius;` | Object data is protected from outside interference |

---

## Building and Running

This project is a single source file with no `.csproj`, so compile it directly with the
C# compiler:

```bash
csc Program.cs        # produces Program.exe
Program.exe           # run it
```

To build it as a .NET SDK project instead, generate a project file first:

```bash
dotnet new console -o . --force
dotnet run
```

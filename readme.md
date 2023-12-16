<h1 style="text-align: center;">Geo-Wall-E</h1>

> Graphical application made with WinForms

## Table of content

### Requirements

- .Net 7.0
- Git
- Windows SDK for compiling

### What it can do?

- Can declare figures like:
  - \<TypeOfFigure> \<identifier>(\<point>, \<radius>);

  - Available types
    - circle, line, segment, ray, arc

- Can declare global consts with the const keyword
  - const \<constIdentifier> = \<expression\value>
  - Sequences like: const \<sequenceIdentifier> = {\<sequenceValues>}

- Can draw figures using DrawKeyword
  - draw \<figure>
  - draw \<figureSequence>

- Temporal variables using let-in expressions
  - let x = 5 in x;
  - Multiple variables declaration with:

    ```csharp
    let -> {
        
        \<identifier> = value,
        
        \<identifier> = value,
        
        \<identifier> = value,
        ...
    } in \<expression>;
    ```


using Tokens;
using Nodes;
using System.Reflection;
using Lists;

namespace ParserAnalize;
public partial class Parser
{
    #region Figures
    private Node ParseFigure()
    {
        var type = CurrentToken?.Value;
        return type switch
        {
            "line" => ParseLine,
            "segment" => ParseSegment,
            "ray" => ParseRay,
            "circle" => ParseCircle,
            "arc" => ParseArc,
            _ => throw new Exception($"It's not a figure {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}. How do you even get here?"),
        };
    }
    private Node ParseArc
    {
        get
        {
            // arc arc(p1, p2, p3, m) "It's an arc!";
            _ = ConsumeToken(TokenType.Figure);
            var name = CurrentToken?.Type == TokenType.LParen ? "" : ConsumeToken(TokenType.Identifier).Value;
            if (CurrentToken?.Type == TokenType.LParen)
            {
                _ = ConsumeToken(TokenType.LParen);
                var p1 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var p2 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var p3 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var m = ParseExpression();
                _ = ConsumeToken(TokenType.RParen);
                object comment = null!;
                if (CurrentToken?.Type == TokenType.StringLiteral)
                {
                    comment = ParseStringLiteral;
                }
                return new ArcNode(name, p1, p2, p3, m, (Node)comment);
            }
            else
            {
                return new ArcNode(name, null!, null!, null!, null!, null!);
            }
        }
    }
    private Node ParseCircle
    {
        //circle cir(p1, r) "It's a circle!"
        get
        {
            _ = ConsumeToken(TokenType.Figure);
            var name = CurrentToken?.Type == TokenType.LParen ? "" : ConsumeToken(TokenType.Identifier).Value;
            if (CurrentToken?.Type == TokenType.LParen)
            {
                _ = ConsumeToken(TokenType.LParen);
                var p1 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var r = ParseExpression();
                _ = ConsumeToken(TokenType.RParen);
                object comment = null!;
                if (CurrentToken?.Type == TokenType.StringLiteral)
                {
                    comment = ParseStringLiteral;
                }
                return new CircleNode(name, p1, r, (Node)comment);
            }
            else
            {
                return new CircleNode(name, null!, null!, null!);
            }

        }
    }
    private Node ParseRay
    {
        get
        {
            // ray ray(p1, p2) "It's a ray!";
            _ = ConsumeToken(TokenType.Figure);
            var name = CurrentToken?.Type == TokenType.LParen ? "" : ConsumeToken(TokenType.Identifier).Value;
            if (CurrentToken?.Type == TokenType.LParen)
            {
                _ = ConsumeToken(TokenType.LParen);
                var p1 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var p2 = ParseExpression();
                _ = ConsumeToken(TokenType.RParen);
                object comment = null!;
                if (CurrentToken?.Type == TokenType.StringLiteral)
                {
                    comment = ParseStringLiteral;
                }
                return new RayNode(name, p1, p2, (Node)comment);
            }
            else
            {
                return new RayNode(name, null!, null!, null!);
            }
        }
    }
    private Node ParseSegment
    {
        get
        {
            // segment seg(p1, p2) "It's a segment!";
            _ = ConsumeToken(TokenType.Figure);
            var name = CurrentToken?.Type == TokenType.LParen ? "" : ConsumeToken(TokenType.Identifier).Value;
            if (CurrentToken?.Type == TokenType.LParen)
            {
                _ = ConsumeToken(TokenType.LParen);
                var p1 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var p2 = ParseExpression();
                _ = ConsumeToken(TokenType.RParen);
                object comment = null!;
                if (CurrentToken?.Type == TokenType.StringLiteral)
                {
                    comment = ParseStringLiteral;
                }
                return new SegmentNode(name, p1, p2, (Node)comment);
            }
            else
            {
                return new SegmentNode(name, null!, null!, null!);
            }
        }
    }
    private Node ParseLine
    {
        get
        {
            // line ln (p1, p2) "It's a line!";
            _ = ConsumeToken(TokenType.Figure);
            var name = CurrentToken?.Type == TokenType.LParen ? "" : ConsumeToken(TokenType.Identifier).Value;
            if (CurrentToken?.Type == TokenType.LParen)
            {
                _ = ConsumeToken(TokenType.LParen);
                var p1 = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                var p2 = ParseExpression();
                _ = ConsumeToken(TokenType.RParen);
                object comment = null!;
                if (CurrentToken?.Type == TokenType.StringLiteral)
                {
                    comment = ParseStringLiteral;
                }
                return new LineNode(name, p1, p2, (Node)comment);
            }
            else
            {
                return new LineNode(name, null!, null!, null!);
            }
        }
    }
    private Node ParseColor
    {
        get
        {
            _ = ConsumeToken(TokenType.ColorKeyword);
            var name = ConsumeToken(TokenType.Identifier);

            // Get the type of the Brushes class
            var brushesType = typeof(Brushes);

            // Get the property with the given name
            var brushProperty = brushesType.GetProperty(name.Value, BindingFlags.Public | BindingFlags.Static);

            // Check if the property exists and is of the right type
            if (brushProperty != null && brushProperty.PropertyType == typeof(Brush))
            {
                // The name coincides with a brush color
                var brush = brushProperty.GetValue(null) as Brush;
                // Use the brush...
                LE.Color.Push(brush!);
            }
            else
            {
                // The name doesn't coincide with a brush color
                throw new Exception($"Undefined color {name.Value} at line {CurrentToken?.Line} and column {CurrentToken?.Column}");
            }

            return new EndNode();
        }
    }
    private Node ParseRestore
    {
        get
        {
            _ = ConsumeToken(TokenType.RestoreKeyword);
            if (LE.Color.Count > 1) { LE.Color.Pop(); }
            return new EndNode();
        }
    }
    private Node ParseDraw
    {
        get
        {
            _ = ConsumeToken(TokenType.DrawKeyword);
            var toDraw = ParseExpression();
            return toDraw is Figure fg && fg.name is ""
                ? new DrawNode(toDraw, true)
                : new DrawNode(toDraw, false);
        }
    }
    private Node ParseMeasure
    {
        get
        {
            //measure(p1, p2);
            _ = ConsumeToken(TokenType.MeasureKeyword);
            //Point 1
            _ = ConsumeToken(TokenType.LParen);
            var p1 = (PointNode)ParseExpression();
            if (p1 is not PointNode) throw new Exception($"Expected token {TokenType.Point}, but found {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}");

            //Comma
            _ = ConsumeToken(TokenType.Comma);
            //Point 2 
            var p2 = (PointNode)ParseExpression();
            if (p2 is not PointNode) throw new Exception($"Expected token {TokenType.Point}, but found {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}");

            _ = ConsumeToken(TokenType.RParen);
            return new MeasureNode(p1, p2);

        }
    }
    #endregion

}
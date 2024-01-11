using Tokens;
using Nodes;
using System.Reflection;
using Lists;

namespace ParserAnalize;
public partial class Parser
{
    #region Figures
    private Node ParsePointExpression
    {
        get
        {
            _ = ConsumeToken(TokenType.Point);
            string name = CurrentToken?.Type == TokenType.LParen ? "" : ConsumeToken(TokenType.Identifier).Value;
            Node x;
            Node y;
            if (CurrentToken?.Type == TokenType.LParen)
            {
                _ = ConsumeToken(TokenType.LParen);
                x = ParseExpression();
                _ = ConsumeToken(TokenType.Comma);
                y = ParseExpression();
                _ = ConsumeToken(TokenType.RParen);
            }
            else
            {
                x = null!;
                y = null!;
            }

            return new PointNode(name, x, y);
        }

    }
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
            return new ColorNode(name.Value);
        }
    }
    private Node ParseRestore
    {
        get
        {
            _ = ConsumeToken(TokenType.RestoreKeyword);
            return new RestoreNode();
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
            var p1 = ParseExpression();
            //Comma
            _ = ConsumeToken(TokenType.Comma);
            //Point 2 
            var p2 = ParseExpression();
            _ = ConsumeToken(TokenType.RParen);
            return new MeasureNode(p1, p2);

        }
    }
    private Node ParseIntersect
    {
        get
        {
            //intersect(seg(p1, p2), seg(p3, p4));
            _ = ConsumeToken(TokenType.IntersectKeyword);
            _ = ConsumeToken(TokenType.LParen);
            var figure1 = ParseExpression();
            _ = ConsumeToken(TokenType.Comma);
            var figure2 = ParseExpression();
            _ = ConsumeToken(TokenType.RParen);
            return new IntersectNode(figure1, figure2);
        }
    }
    #endregion
}
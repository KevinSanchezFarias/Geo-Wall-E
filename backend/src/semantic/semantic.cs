using System.Linq;
using Lists;
using Nodes;

namespace WaLI.backend.src.semantic
{
    public class SemanticAnalyzer
    {
        //TODO: Add symbol table
        public void Analyze(Node node)
        {
            /* switch (node)
             {
                 case ConstDeclarationNode constDeclarationNode:
                     AnalyzeConstDeclaration(constDeclarationNode);
                     break;
                 case Figure figure:
                     AnalyzeFigure(figure);
                     break;
                 case SequenceNode sequenceNode:
                     AnalyzeSequence(sequenceNode);
                     break;
                 case IntersectNode intersectNode:
                     AnalyzeIntersect(intersectNode);
                     break;
                 case MultiAssignmentNode multiAssignmentNode:
                     AnalyzeMultiAssignment(multiAssignmentNode);
                     break;
                 case GlobalConstNode globalConstNode:
                     AnalyzeGlobalConst(globalConstNode);
                     break;
                 case VariableDeclarationNode variableDeclarationNode:
                     AnalyzeVariableDeclaration(variableDeclarationNode);
                     break;
                 case MeasureNode measureNode:
                     AnalyzeMeasure(measureNode);
                     break;
                 case IdentifierNode identifierNode:
                     AnalyzeIdentifier(identifierNode);
                     break;
                 case FunctionCallNode functionCallNode:
                     AnalyzeFunctionCall(functionCallNode);
                     break;
                 case FunctionDeclarationNode functionDeclarationNode:
                     AnalyzeFunctionDeclaration(functionDeclarationNode);
                     break;
                 case FunctionPredefinedNode functionPredefinedNode:
                     AnalyzeFunctionPredefined(functionPredefinedNode);
                     break; 
                 default:
                     break;
                     throw new Exception($"Unexpected node type {node.GetType()}");
             }*/
        }

        private void AnalyzeFunctionPredefined(FunctionPredefinedNode functionPredefinedNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeFunctionDeclaration(FunctionDeclarationNode functionDeclarationNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeFunctionCall(FunctionCallNode functionCallNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeIdentifier(IdentifierNode identifierNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeMeasure(MeasureNode measureNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeVariableDeclaration(VariableDeclarationNode variableDeclarationNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeGlobalConst(GlobalConstNode globalConstNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeMultiAssignment(MultiAssignmentNode multiAssignmentNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeIntersect(IntersectNode intersectNode)
        {
            throw new NotImplementedException();
        }

        private void AnalyzeConstDeclaration(ConstDeclarationNode constDeclarationNode)
        {
            // Check if the identifier is already declared in the current scope
            if (LE.DeclaredConst.Any(node => node.Identifier == constDeclarationNode.Identifier))
            {
                throw new Exception($"Identifier {constDeclarationNode.Identifier} is already declared in the current scope");
            }

            // If the identifier is not already declared, add it to the symbol table
            //evariables[constDeclarationNode.Identifier] = null; // Or some default value

            // Then, analyze the expression being assigned to the identifier
            Analyze(constDeclarationNode.Value);
        }

        private void AnalyzeFigure(Figure node)
        {
            // Analyze a figure node
            // This might involve checking if the figure is well-formed
        }

        private void AnalyzeSequence(SequenceNode node)
        {
            // Analyze a sequence node
            // This might involve analyzing each statement in the sequence
        }

        // Add methods to analyze other node types...
    }
}
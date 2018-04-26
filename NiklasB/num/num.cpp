#include "stdafx.h"
#include "IExpression.h"
#include "Parser.h"

void AddDefinition(DefinitionList& globals, Lexer& lexer);
void PrintDefinition(Definition const& def);
void ReadParamList(Lexer& lexer, std::vector<std::string>& names);
void Evaluate(NameMap const& globals, Lexer& lexer);
void PrintError(ExpressionException const& e);
void ShowHelp();

int main(int argc, char** argv)
{
    DefinitionList globals;

    std::string input;

    fputs("Num expression evaluator. Type 'help' for usage.\n", stdout);

    for (;;)
    {
        fputs("\n> ", stdout);

        input.clear();
        std::getline(std::cin, input);

        try
        {
            Lexer lexer{ StringRef{ input } };

            if (lexer.GetTokenType() == TokenType::None)
            {
                continue;
            }
            else if (lexer.GetTokenType() == TokenType::Name)
            {
                if (lexer.GetName() == "q")
                {
                    break;
                }
                else if (lexer.GetName() == "help")
                {
                    ShowHelp();
                }
                else if (lexer.GetName() == "def")
                {
                    lexer.Advance();
                    AddDefinition(globals, lexer);
                }
                else if (lexer.GetName() == "defs")
                {
                    for (auto& def : globals.vec)
                    {
                        PrintDefinition(*def);
                    }
                }
                else
                {
                    Evaluate(globals.map, lexer);
                }
            }
            else
            {
                Evaluate(globals.map, lexer);
            }
        }
        catch (ExpressionException const& e)
        {
            PrintError(e);
        }
    }

    return 0;
}

void Evaluate(NameMap const& globals, Lexer& lexer)
{
    Parser parser{ lexer, globals };
    ExpressionPtr parsedExpr = parser.ParseFullExpression();

    ExpressionContext context;
    Number value = parsedExpr->Evaluate(context);

    printf("%s = ", lexer.GetSource().c_str());
    PrintNumber(value);
    printf("\n");
}

void PrintNumber(Number value)
{
    if (value.isDouble)
    {
        printf("%lf", value.doubleValue);
    }
    else
    {
        printf("%I64d", value.intValue);
    }
}

void AddDefinition(DefinitionList& globals, Lexer& lexer)
{
    if (lexer.GetTokenType() != TokenType::Name)
    {
        lexer.Fail("Name expected after 'def'.");
    }

    auto def = std::make_shared<Definition>();
    def->name = lexer.GetName();
    lexer.Advance();

    if (lexer.GetTokenType() == TokenType::None)
    {
        auto p = globals.map.find(def->name);
        if (p != globals.map.end())
        {
            PrintDefinition(*p->second);
        }
        else
        {
            printf("No definition for %s.\n", def->name.c_str());
        }
        return;
    }

    if (lexer.GetTokenType() == TokenType::StartGroup)
    {
        // It's a function definition.
        def->isFunction = true;

        // Read the parameter names.
        ReadParamList(lexer, def->paramNames);
    }

    // We expect a colon before the definition.
    if (lexer.GetTokenType() != TokenType::Lamda)
    {
        lexer.Fail("'=>' expected.");
    }
    lexer.Advance();

    // Parse the expression.
    Parser parser{ lexer, globals.map, def };
    def->expression = parser.ParseFullExpression();

    // Add the definition to the definition list.
    globals.vec.push_back(def);
    globals.map[def->name] = std::move(def);
}

void ReadParamList(Lexer& lexer, std::vector<std::string>& names)
{
    assert(lexer.GetTokenType() == TokenType::StartGroup);
    lexer.Advance();

    if (lexer.GetTokenType() == TokenType::EndGroup)
    {
        lexer.Advance();
        return;
    }

    if (lexer.GetTokenType() != TokenType::Name)
    {
        lexer.Fail("Expected name or ')' after '('.");
    }

    for (;;)
    {
        assert(lexer.GetTokenType() == TokenType::Name);
        names.push_back(lexer.GetName());
        lexer.Advance();

        if (lexer.GetTokenType() == TokenType::EndGroup)
        {
            lexer.Advance();
            break;
        }

        if (lexer.GetTokenType() != TokenType::Comma)
        {
            lexer.Fail("Expected ',' or ')' after name.");
        }
        lexer.Advance();

        if (lexer.GetTokenType() != TokenType::Name)
        {
            lexer.Fail("Expected name after ','.");
        }
    }
}

void PrintDefinition(Definition const& def)
{
    printf("def %s", def.name.c_str());

    // If it's a function, print the parameter list.
    if (def.isFunction)
    {
        fputc('(', stdout);

        size_t const paramCount = def.paramNames.size();
        for (size_t i = 0; i < paramCount; ++i)
        {
            if (i != 0)
            {
                fputs(", ", stdout);
            }
            fputs(def.paramNames[i].c_str(), stdout);
        }

        fputc(')', stdout);
    }

    fputs(" => ", stdout);

    def.expression->Print();

    fputc('\n', stdout);
}

void PrintError(ExpressionException const& e)
{
    printf(
        "Error: %s\n"
        "       %s\n"
        "       ", 
        e.GetMessage(), 
        e.GetSource()
        );

    for (int c = e.GetCharIndex(); c > 0; --c)
    {
        fputc(' ', stdout);
    }

    printf("^\n");
}

void ShowHelp()
{
    static const char message[] =
        "q\n"
        "    Quit.\n"
        "\n"
        "help\n"
        "    Show help.\n"
        "\n"
        "<expression>\n"
        "    Evalute expression.\n"
        "\n"
        "def <name> => <expression>\n"
        "    Define variable with value equal to <expression>.\n"
        "\n"
        "def <name>(<params>) => <expression>\n"
        "    Define function with the specified parameters.\n"
        "\n"
        "defs\n"
        "    List all definitions\n"
        "\n"
        "def <name>\n"
        "    List specific definition.\n"
        "\n"
        "Example definitions:\n"
        "    def sqrt(n) => n ** 0.5\n"
        "    def is_prime_helper(n, f) => f * f > n ? 1 : n % f = 0 ? 0 : is_prime_helper(n, f + 2)\n"
        "    def is_prime(n) => n < 3 ? n = 2 : (n & 1) = 0 ? 0 : is_prime_helper(n, 3)\n"
        ;

    fputs(message, stdout);
}

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    public class CodeGener
    {
        private CodeCompileUnit _unit;

        private CodeTypeDeclaration _newClass;

        private CodeNamespace _codeNameSpace;

        public CodeGener(string nameSpace, string name, TypeAttributes typeAttr = TypeAttributes.Public, bool isClass = true)
        {
            _unit = new CodeCompileUnit();

            _newClass = new CodeTypeDeclaration(name);
            _newClass.TypeAttributes = typeAttr;
            _newClass.IsClass = true;
            _codeNameSpace = new CodeNamespace(nameSpace);

            _codeNameSpace.Types.Add(_newClass);
            _unit.Namespaces.Add(_codeNameSpace);
        }

        public CodeGener AddImport(params string[] importNames)
        {
            foreach (string importName in importNames)
            {
                CodeNamespaceImport import = new CodeNamespaceImport(importName);
                _codeNameSpace.Imports.Add(import);
            }

            return this;
        }

        public void AddBaseType(params string[] baseTypeNamse)
        {
            foreach (string s in baseTypeNamse)
            {
                _newClass.BaseTypes.Add(s);
            }
        }


        /// <summary>
        /// 添加成员域
        /// </summary>
        /// <param name="type">域类型</param>
        /// <param name="fieldName">域名称</param>
        /// <param name="initAct">初始化行为</param>
        /// <param name="memberAttr">域特性</param>
        /// <returns></returns>
        public CodeGener AddMemberField(System.Type type, string fieldName, Action<CodeMemberField> initAct = null, MemberAttributes memberAttr = MemberAttributes.Public)
        {
            CodeMemberField newField = new CodeMemberField(type, fieldName);
            newField.Attributes = memberAttr;
            _newClass.Members.Add(newField);
            if (initAct != null)
            {
                initAct(newField);
            }

            return this;
        }

        /// <summary>
        /// 添加成员属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">字段名</param>
        /// <param name="initAct">初始化行为</param>
        /// <param name="memberAttr">成员特性</param>
        /// <returns></returns>
        public CodeGener AddMemberProperty(System.Type type, string propertyName, Action<CodeMemberProperty> initAct = null, MemberAttributes memberAttr = MemberAttributes.Public)
        {
            string fieldName = "_" + propertyName;
            AddMemberField(type, fieldName, null, MemberAttributes.Private);

            CodeMemberProperty newProperty = new CodeMemberProperty();
            newProperty.Type = new CodeTypeReference(type);
            newProperty.Name = propertyName;
            newProperty.Attributes = memberAttr;
            _newClass.Members.Add(newProperty);

            newProperty.HasGet = true;
            newProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
            newProperty.HasSet = true;
            newProperty.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));

            if (initAct != null)
            {
                initAct(newProperty);
            }

            return this;
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="outputPath">生成目录</param>
        public void GenCSharp(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            string outputFilePath = Path.Combine(outputPath, _newClass.Name) + ".cs";
            //生成代码
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            //缩进样式
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            //空行
            options.BlankLinesBetweenMembers = true;

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFilePath))
            {
                Debug.Log("生成代码" + outputFilePath);
                provider.GenerateCodeFromCompileUnit(_unit, sw, options);
            }

            AssetDatabase.Refresh();
        }
    }

    public static class CodeEx
    {
        /// <summary>
        /// 添加注释
        /// </summary>
        /// <param name="member"></param>
        /// <param name="comment"></param>
        /// <param name="isDoc"></param>
        public static void AddComment(this CodeTypeMember member, string comment, bool isDoc = false)
        {
            member.Comments.Add(new CodeCommentStatement(new CodeComment(comment, isDoc)));
        }
    }

    public static class CodeTypeMemberEx
    {
        /// <summary>
        /// 添加不带参数的属性
        /// </summary>
        /// <param name="member"></param>
        /// <param name="AttributeType"></param>
        public static void AddMemberCostomAttribute<T>(this CodeTypeMember member)
        {
            System.Type AttributeType = typeof(T);
            member.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(AttributeType)));
        }

        public static void AddMemberCostomAttribute(this CodeTypeMember member, string attr)
        {
            member.CustomAttributes.Add(new CodeAttributeDeclaration(attr));
        }

        /// <summary>
        /// 添加带有参数的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <param name="argExpressions"></param>
        public static void AddMemberCostomAttribute<T>(this CodeTypeMember member, params string[] argExpressions) where T : Attribute
        {
            System.Type AttributeType = typeof(T);
            CodeAttributeArgument[] args = new CodeAttributeArgument[argExpressions.Length];


            for (int i = 0; i < argExpressions.Length; i++)
            {
                args[i] = new CodeAttributeArgument(new CodeSnippetExpression(argExpressions[i]));
            }


            member.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(AttributeType), args));
        }

        public static void AddMemberCostomAttribute(this CodeTypeMember member, string attr, params string[] argExpressions)
        {
            CodeAttributeArgument[] args = new CodeAttributeArgument[argExpressions.Length];

            for (int i = 0; i < argExpressions.Length; i++)
            {
                args[i] = new CodeAttributeArgument(new CodeSnippetExpression(argExpressions[i]));
            }

            member.CustomAttributes.Add(new CodeAttributeDeclaration(attr, args));
        }

        /// <summary>
        /// 域的初始化表达式
        /// </summary>
        /// <param name="field"></param>
        /// <param name="express">表达式</param>
        public static void AddFieldMemberInit(this CodeMemberField field, string express)
        {
            field.InitExpression = new CodeSnippetExpression(express);
        }
    }
}
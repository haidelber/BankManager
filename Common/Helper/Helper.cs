using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace BankManager.Common.Helper
{
    public static class Helper
    {
        public static PropertyInfo GetPropertyFromExpression<T, T2>(Expression<Func<T, T2>> getPropertyLambda)
        {
            MemberExpression Exp = null;

            //this line is necessary, because sometimes the expression comes in as Convert(originalexpression)
            if (getPropertyLambda.Body is UnaryExpression)
            {
                var unExp = (UnaryExpression)getPropertyLambda.Body;
                if (unExp.Operand is MemberExpression)
                {
                    Exp = (MemberExpression)unExp.Operand;
                }
                else
                    throw new ArgumentException();
            }
            else if (getPropertyLambda.Body is MemberExpression)
            {
                Exp = (MemberExpression)getPropertyLambda.Body;
            }
            else
            {
                throw new ArgumentException();
            }

            var result = (PropertyInfo)Exp.Member;

            var sub = Exp.Expression;

            while (sub is MemberExpression)
            {
                Exp = (MemberExpression)sub;
                result = (PropertyInfo)Exp.Member;
                sub = Exp.Expression;
            }

            return result;
        }

        public static string FileRename(string filePath, string newName, bool preserveExtension = true)
        {
            if (File.Exists(filePath))
            {
                var dir = Path.GetDirectoryName(filePath);
                var extension = Path.GetExtension(filePath);
                var newPath = Path.Combine(dir, preserveExtension ? $"{newName}{extension}" : newName);
                File.Move(filePath, newPath);
                return newPath;
            }
            return null;
        }

        public static void EnsureDirecory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
        public static void EnsureFile(string filePath)
        {
            EnsureDirecory(Path.GetDirectoryName(filePath));
            using (File.Create(filePath))
            {

            }
        }
    }
}
﻿using System;
using System.Text;

namespace TankLibHelper {
    public class BuilderConfig {
        public string Namespace;
    }
    
    public abstract class ClassBuilder {
        protected string Name;
        protected readonly StructuredDataInfo Info;
        protected readonly BuilderConfig Config;

        protected ClassBuilder(BuilderConfig config, StructuredDataInfo info) {
            Config = config;
            Info = info;
        }
        
        public virtual string GetName() {
            return Name;
        }

        public virtual string BuildCSharp() {
            throw new NotImplementedException();
        }

        protected void WriteDefaultHeader(StringBuilder builder, string type, string builderName) {
            builder.AppendLine($"// {type} generated by {builderName}");
            builder.AppendLine("");

            builder.AppendLine(@"// ReSharper disable All");
            builder.AppendLine($"namespace {Config.Namespace} {{");
        }
        
        protected string GetDefaultHeader(string type, string builderName, string imports) {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"// {type} generated by {builderName}");
            builder.AppendLine($"{imports}");

            builder.AppendLine(@"// ReSharper disable All");
            builder.AppendLine($"namespace {Config.Namespace} {{");
            return builder.ToString();
        }
    }
}
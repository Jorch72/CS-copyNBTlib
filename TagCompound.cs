﻿using System;
using System.Collections.Generic;
using System.IO;

namespace copyNBTlib
{
	public class TagCompound : TagBase, IDictionary<string, TagBase>
	{
		readonly IDictionary<string, TagBase> _tags = new Dictionary<string, TagBase>();

		public TagCompound()
			: base(TagType.Compound) {  }


		#region TagBase implementation (read / write payload)

		public override void ReadPayload(BinaryReader reader)
		{
			Clear();
			TagType type;
			while ((type = ReadTagType(reader)) != TagType.End) {
				ValidateTagTypeIDE(type);
				var name = ReadString(reader);
				var tag = CreateTagFromType(type);
				tag.ReadPayload(reader);
				this[name] = tag;
			}
		}

		public override void WritePayload(BinaryWriter writer)
		{
			foreach (var nameTagPair in this) {
				var name = nameTagPair.Key;
				var tag = nameTagPair.Value;
				tag.WriteTagType(writer);
				WriteString(writer, name);
				tag.WritePayload(writer);
			}
			WriteTagType(writer, TagType.End);
		}

		#endregion


		#region IDictionary implementation

		public ICollection<string> Keys { get { return _tags.Keys; } }

		public ICollection<TagBase> Values { get { return _tags.Values; } }

		public override TagBase this[string name] {
			get { return _tags[name]; }
			set { _tags[name] = value; }
		}


		public bool ContainsKey(string name)
		{
			return _tags.ContainsKey(name);
		}

		public bool TryGetValue(string name, out TagBase tag)
		{
			return _tags.TryGetValue(name, out tag);
		}

		public override void Add(string name, TagBase tag)
		{
			_tags.Add(name, tag);
		}

		public override bool Remove(string name)
		{
			return _tags.Remove(name);
		}

		#endregion

		#region ICollection implementation

		public bool IsReadOnly { get { return false; } }

		public override int Count { get { return _tags.Count; } }


		public bool Contains(KeyValuePair<string, TagBase> item)
		{
			return _tags.Contains(item);
		}

		public void Add(KeyValuePair<string, TagBase> item)
		{
			_tags.Add(item);
		}

		public bool Remove(KeyValuePair<string, TagBase> item)
		{
			return _tags.Remove(item);
		}

		public override void Clear()
		{
			_tags.Clear();
		}

		public void CopyTo(KeyValuePair<string, TagBase>[] array, int arrayIndex)
		{
			_tags.CopyTo(array, arrayIndex);
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<string, TagBase>> GetEnumerator()
		{
			return _tags.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}


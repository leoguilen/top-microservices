using System;
using System.Linq;

namespace TOP.ProfileService.Domain.ValueObject
{
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            //compare all public properties
            var publicProperties = GetType().GetProperties();

            if (publicProperties.Any())
            {
                return publicProperties.All(p =>
                {
                    var left = p.GetValue(this, null);
                    var right = p.GetValue(other, null);


                    return left != null && (left is TValueObject ? ReferenceEquals(left, right) : left.Equals(right));
                });
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is ValueObject<TValueObject> item && Equals((TValueObject)item);
        }

        public override int GetHashCode()
        {
            var hashCode = 31;
            var changeMultiplier = false;
            const int index = 1;

            //compare all public properties
            var publicProperties = GetType().GetProperties();


            if (!publicProperties.Any()) return hashCode;

            foreach (var item in publicProperties)
            {
                var value = item.GetValue(this, null);

                if (value != null)
                {

                    hashCode = hashCode * (changeMultiplier ? 59 : 114) + value.GetHashCode();

                    changeMultiplier = !changeMultiplier;
                }
                else
                {
                    hashCode ^= index * 13;//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                }
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }
    }
}

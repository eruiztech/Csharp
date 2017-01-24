/* Edgar Ruiz
 * CS 431
 * December 5, 2016
 */

using System;

namespace CosmosKernel1
{
    public class Variable
    {
        public String name;
        public String value;

        public Variable(String n, String v)
        {
            name = n;
            value = v;
        }

        public void setName(String n)
        {
            name = n;
        }

        public void setValue(String v)
        {
            value = v;
        }

        public String getName()
        {
            return name;
        }

        public String getValueString()
        {
            return value;
        }

        public Int32 getValueNumeric()
        {
            return Int32.Parse(value);
        }
    }

}


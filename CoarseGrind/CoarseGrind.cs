﻿// Copyright (c) 2019, 2016 William Arthur Hood
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.IO;
using Rockabilly.Common;
using Rockabilly.HtmlEffects;
using Rockabilly.IO;

namespace Rockabilly.CoarseGrind
{
	public static class CoarseGrind
	{
		internal static string DateFormatString = "yyyy-MM-dd HH-mm-ss.ffff";
		internal const string SUMMARY_FILE = "SUMMARY";
		internal const string SUMMARY_FILE_EXTENSION = ".csv";
		internal const string SUMMARY_FILE_NAME = SUMMARY_FILE + SUMMARY_FILE_EXTENSION;
		internal static readonly string SUMMARY_TEXTFILE_NAME = '.' + SUMMARY_FILE;
		internal static bool KILL_SWITCH = false;

        internal static string LogHeader(string title)
        {
            return String.Format("<table><tr><td>\r\n\r\n{0}\r\n\r\n</td><td><h1>{1}</h1>\r\nInitiated {2}</td></tr></table>\r\n<hr>\r\n<small><i>Powered by Coarse Grind</i></small>\r\n\r\n", HEADER.ToString(), title, HeaderTimeStamp);
        }

        public static readonly Icon_Header HEADER = new Icon_Header();

        public class Icon_Header : InlineImage
        {
            public override string Base64ImageData
            {
                get
                {
                    return "iVBORw0KGgoAAAANSUhEUgAAAEYAAABGCAYAAABxLuKEAAAzKnpUWHRSYXcgcHJvZmlsZSB0eXBlIGV4aWYAAHjarZxZliQ5kl3/sYpeAmZAloPxHO6Ay+d9UI/IoSq7m2xmVIWHu5uZqgIibxAI4M7//l/X/cd//EcIzVeXS+vVavX8ly1bHPyj+++/8f4OPr+/33+5/vwu/PXn7vcvIj9KfE3ft/3Xzw8/j7w+/vx8/XzO4OflTx9k5+cX86+/GD8fFPvPBX5+/utCKXwX8Pvng8bPB6X4c+X8fT9/rlyttz8/ws/7/P35/RsG/u/0V04t1lJDy/ydo2+tGv/u0efGuG3dqG2f/zRu//K9+/XSyD3Fk0Ly7+/43WXiEZKlwdf2/g7x++ng552/Y2rf3TqmjFvg3/Yztj+D+k///dPduz/f/s+0/2Vaf/8r/MPPf2bb/ZqFGn9+kf42S/X313/781B+fdDfpvXN3Z+uXPvvK//l53nG9edHdr+mT/+/d/d7z/d0I1ceuf481K9Hef/idVOj9d5V+aMUIQg7/9Af409nOhextP3ykz8rWIhM5Q057DDCDed9XWFxizme2Pga43IxvR92JsPietOd9Sfc2Jj4zSTHtAiJxE/j73sJ77L2LrdC99v5HXhpDHwY0fP//sf9d194X06E4PvvseK+oqKOu/CB6dcXXsaMhPszqOUN8K8/f/9P85qYwfKGufOAw0/3fcQs4Y/gSm+iEy8sfP0yKrT98wEMEZcu3ExIzICvIZVQuaMWYwuBgexM0ODWY8pxMgOhlLi5yZhTqkwOOcy1eU8L76WxxO/HgGHKLpVUSciu1GSyci7ET8udGBollVxKqaWVXqyMmqoyrNZWhaqjpZZbabW11l2zNnrquZdee+u9Wx8WLYG6xchH62Y2BhcdfPLg3YMXjDHjTDPPMutss09zcyzCZ+VVVl1t9WVr7LjTJo933W33bXuccAilk0859bTTj51xCbWbbr7l1tvc7dfu+D1rP9P6L3/+L2Yt/MxafDOlF7bfs8ZPW/v1EUFwUjRnzFjMgQlvzBozRmBrznwPOUfNnObMWyQrSuQmiyZnB80YM5hPiOWG33P3M3OOUfz/Mm+u9Tdv8X86c05T99+cuX+dt383a1u0st6MfWmoQfWJ7OP3p4/Yh9jyH786i2E2G6esQeIsizAuMd9H6Sft1sec4fZ2y7B2dzkW2rkW7FrdtoglgzZqKy4UgJCJuCGm0yrvmQzmaqNY5v0Md5grXlgqm4+pdj6xt8o91h3zsrsh9RSH64zGtOsvA1YrQ3lmbhvCKvw6r1nXGcTJOQzsBmTJ33FGKb0VD6CKG2YuIbsiuucf/9Ovf/ugVpArtwMx+TJiPcQd2wRXarhp3jbOSsxb2iHftPrgOXigg3ZxPYVmk0lro+25GEDi6pS980phg/8nM2ret74qr/MnrZJGnyOtMEozBt96CN3xEWDdMcJin1tOvyf029Pyo5GGt+V4ba5V77U2+eVGP9wWLhfhcgzXIKbCcaHvSsxlBWixwx2cWxWc8yJJSAYmBZ7aXGikGZiaulNbJNWM+kUqZSqWAP8K+XYESplbkWKXESOWNje+Ig/DBe+dRDlxMIl5KzXdOoxRJJBuu2H30hxR2Hq5uTLRzVJts/lJBPCYCrduY3WiId1A7LS6Jjqp8zG6fF33FGBjgDLAiNdAhIXaGKfXPW5iFPfYK2/UrokLgKygoDfSgR8QwrWTrzB1beOuxWi6OmHew/j0Wzp/n7NnSDMxFFAww9ECtH34DAaIN2UCOmW7ca4LWp96BUG7u94bKLOJ1LFbAB/2YfY6HxsIXgmGykOhLgrCePYS5myd8UyEdK/JNJ7EWnfGjA0DCvtmAhjjlBeq4RCArZAhiVlce5OFeRymdpGNPGGdZRC0zY4m8q7jphkvMjSPRBXhlZDqYTKuKE5fDykMEu1b2gTfOh9Aku11JVf8njee1qVm3Ln5XG8DYDxAALG0ScJ2+QjuKkxmy2YmHLlQESocPmVfsI2J7wp5I0YPBMk8pTN5Ds113XcZU7LmyiM1qyv3MAeome3wrw08MUg8Xhq1EHTM0slplOrO1D3vWjpAiE5DcElKwSN19UM0zrIviUZygHXMMTd4wN+BipzGg61jaMQKZTM2qxNbed1GjJRGQCUQWA9zwFtu4SImVj6a1llvRe2RUHxS6ga7jVWLudwjXMH8eeswVOvEF5hNnvo54p5k+95pAviDYWF89PGLh0I5aMZ83OccqRFuVXo0d5I0J4aRpwYS7HZmHKpri+CDg8qJ3TbTa1wrrAa5jMkY+es30+2gtGYXI5BFfUCS3VRWk0JtnouBpak3IvyEeSCuye8YEhjuEuyMEZA3UVfuNOUw5uIoRkoD7XJfUrLpkkOb5xEl1TWOCUC7kGMhkgmaO7L4k4Q5w4UjzAjM1dwz6lqrx41wj7CBfXDIUx5fGILLpM0BgV5kP1GyUyLlor8REUEKJT7rAH/Xi5J1ByO8b4GH3PQ1LCby8EyZH4OZ7cK5/Q18skIWDQcFNX8GOgDe1VRCiRKRcKTMF7LEXwTGJsGAKSQU83EXuhG+PL28B1+2g3v/QJjoay8ITNjbT14su3dvP0yAN5EM0/fu7ue2v5v+bpkbduex6GVoGZBoI64CVo3athEM2/fWIH8AXAgLk5x0977ioAkbhUKkBH28Oxm6yfrn9GPodhlQpvzUMZ5G4A7+869pL5jNQXFzPF7RzVTlAmSU9yX4hiY6DkElMowgGYDyuJ3MjsDKrX6jwawthsIZxuqkBjF0hg/aOL0rvUnNdt5Qdlzbhn8A+tPSu/coBdKlFswTvV0pAnSQbrEh6HrGYZNzhCpBh95AIuYGSneYlW9TKFDw8XsXxQLvAYwj403uHZf3HosXAPrXVt2oj4a+mvvAbLDZHbbIzEoeaX5fQiBeOhwvkkHmgD4QtSO/IAT4HkfChAWQCbA5exD7/E92fyxCHg+JRMqBoCOpirAKZiQf4Ekud50eNm4gAA20DJIgNJMJxOtddXbB17yzjlAAedAdlgVV90HZ8hahNmCdgkMLgAIk7knSt4FZhKrgWPDGlHPlTLCZ72pB3fGSTpJFII0foaQ9yaZ4cujZgDyEYAGqMxEUj0+mpvtwF4Lqaqeh+ECbg86eW3rnFX78bA29tHKrbg0wvQtoN7p9gdYJoULuZz6nFbQmzCRNRbgPUpanqTFsnvxAtSjqCakxQY6Y53nAkKscRpMNjS+jVwFr6BCsIUcSIhqJuXmH6BvaEcFWZW2EdZApjNFGhoNHF4seAoSN3y2CcUAj8PxI55BOJOxwMLsw60mCdXWkOiNBrDWL0BETSNiRHXjQhlxZdtBJG7SM58vBi7doGXDjWsQeigj5fJS5EF4JwnjEj4vkCnOT+8zovcUTkl8oBya2owCyJxw9Ku+EL13KiLevQeJF7C1ZQiYiYha81uNBqEhlQHA1gU53DpjQtiSgt4r2Zvx7RjEVMBf7BQEQ97ABcuQI/ch+ghnuwjui+6pXsqIImBwQiNiXZH2s3xSwATCSzp1yElV6FsMCchiqxeEVdOskHO8lX2qsqD0YcqBAfN0Tysmx48qk6SC1QHZgFZGAotBOFKJ27naMOmKOyVqoSSYDXsZ+dmgViwg1k+QSOhDl0JQVCWJ+QtQNUuKH1g9jpIhhUHKFXrfmYiZUJPHcnlvJRT4NeavnIOEg2ABmGfov5eRRwlNhVYCRc8jU/KpPJXPXXFBYmSJaGM1xNb9ixznsSx7CDcHDm7FS5PTV8MG0HuTw0o/CKiaMqEnpbCZmT77yqitbgGLHH4KQSJNMkpHxv6H7UYD7OODfUgCyCqqXesO4YfYCn3/Pirq1EQkJucsxPhfp/tle4lvQhER0lVNi8FaRO8Cq4Oqrqs4hDdW0mkLY7UU24wF6weG1NgM+2MBe3g2+VkLc4xt85FvytRNuyETSDlT3W4Pba0C7NxcQC3GCJAb0luqRyFhTNBlzX0EoAIdQJZHB9CxsRbgxmQtJT6DwUVHaI2L8kJsjeWKOCQOP4Q5pYmUebItznZJPVfJMs0P4hDt2MskeDNYMIcbDexzxmmV8kdCZp7uaDp4h71dWIyFhb/lr3GBbD6nXuuCkcBoGTz2cIWZ2AkIGqyWeXBaYROEeEIYNTIVP+urElMccqlZpMiVHHgH0QbbVzPOdC+rCa9hDFAyGLfNkF9SqKY+tcGcYyNM1KrdSFjDAkGfcJpHUAWSuAzwgQiNeyak8bTgk1URx4ZMB3xuRTaJ32AWkIVsWaYXmbwCYxg8hL+9nnUm8RB767bjtCToQ5YNsItxjHQvyARMEd3RATJSeZYOxihiRiJBEFyDUZhu+MCmGGnKb+FV68xAH3R5l3hQKSA9AEX+Ei4MkwTq0y9qCPAYCTuNBeFKM9FCpAfBHe+P7GQYeAtRah3DYkgdwG0hCMJwNRROU5SapSi6Bs8JNRxFQUvWj5uVMd9ehZtC4GLq4elWqoPbzAckIzKGsPEwLJIwiNQtRJzAXLcWgN6ENIgLIMrxSAbjx/lG1T/IDNOiCBKw8WG43qE6BxSb9D6FHjNSxO6PH/MG921UPwxt/MwiIoaA7IXbQc0VQjSo/AxCVQ+pooQCicG8qhqL301gZdY199S6Sv8AaHIk7x1wPv7gK4VJK4rqkIx6IAB5abwiY8YCt5NcdZVI/xbyk2RyMhuIoFw0Fv8wlG4mLxNMHFEks6YEuQZtO2ApKEtLihXB32VniXKUNa6of4VzQJyg+FbKSZ/DkvaqvieEAlAcCGtTERj8bZYZSJJYur2PYAA0yIDmTp0V+4r16UCgAKdAgyf8sBAqJqz7K4lYGNDZBsL0wq7GLwEDnkcZkjBDlGlI4DzsXZUw3Y4YaxCePhUSeggXC+wZYGF0CF9lJ2LE1ZKOJedIpOIKiMLobmXzgBBE2GpQJJSpv2Ys374pgkr0Ge5DY0Ao++ysqAWkyA3mbU3EGYQPSvIokg0MIZrDKo8sAPQ/HacbGzbjqlBXzA0+J/NkRRSN7f4p5l8gKyU+vuYkqOwHV8wqRMNwlMMzQC1jqI2HFGKoihKLlNvEMIBOzjYSbrkAWDaSOMaqqW1bEo8svNUNL4QQGoo87Rv2fIzVziC1FNE62kaKILBIKyh68KzNcYukk3Vy2igkEfpIeG8xQGnEa9xRRcIgY4FiUhjE+ccrPAsm9Y2qguwpb49MQgBpEGGuomPCcf0RhwJgQVnkBwQx+/q+UVy0kj77y4a9//P4KlBLnA/MGERFizHEnIDALgWiQhCUyttxSRG5gqqVK5nFEVZJzl69DKZLqINoCvMpWHQNJJj00Q+qQNeyHB0M6RhmpV2E4yF1/noWQRwUVDmKnzOifM8xr6CkkUKL4FeLEwFd5tZXI4PbuHU5CuQmjADatpJAOSR4NLoNOSRxMEnFCRkwkWxJ1qyRCSOMdhm6OqJP7kkL4ihzRxc49X61DglrXAG45cPzkUelY6N4Y2tBArFgZO3BWrgPhuVULkoXZFttQrTYhYRE5CRkbFY3c2YEqYYTOq7kj8jOCDoA9nAIxdHRXHeTjbIVsIHpiJI7yBHwUlUeltxTSBrDJRT4QoCdJ4GjUNHSLq89aweU+ExC08UADS7aK1eVUIE4li8VSgXQEcI283MVjIxnTJZeAfD/WwSg8ENGfy9X6bxNkXDy2HSNFkKR+yMMD8HBQgXjRrQep1LT+kblqv0n1k68QSWgiNYrE10tT7shndPYT5AhD5nab4g+JUXkV/mnL+6AThof2PYBkGLXb8TwLAcjjFzKyCM5gWrQJ+LNIkRRV50688jE/LgrBcU0eCs+LzMNYA7AIGfIIjx/aqoAwUzNBIgfkb5AXLCK9kpZKGCkU2YvItyiTE1qhi0oMrmHCQEtuuiyBUwbYUfJ+uk7grSprDV3VtVCxPGnO44WRyuzcP/CrqjM2S+N4sdx9lfoqj6gixHUs7uIKgYmpKn2NUhGYkAR/pYPHX3oQVQIHpgylwtNAEyiLncfyRpwhN9FXiFGFI0pGYos0gWCDCvZwKHaa2VBFJ8pRYattSsVujJwkfI9TluyIPUAtB/A93YX62FXSDRWAjSJexmRMjtaBwLN8NypPuLpqbiOAhojttvEqVZmL8scO527Bq/kj+YmjrxGr/UreAAGXewVDJjnI7izsaEwyJou4QFNy7zK6DiRH33UhF5A7NzOyAOWw+ak+CxN6EjyHgdHyBYL42VkzyBrRh4vVMkNC+kU9AwZhhs5Q8RoMB9IJiftUwjNVWHXeYUvT9QDXBwlbnB84iBrC+jgDHg40vhYCS0E9vmpbBVbIeYz9awSBaqssEeHJhIst9itygsUMbSWO8BKQQgDL+NysdSjwDrUHp3SwD3YhCIYKYIrxX1bJ+7+aJTWgyC9NFR93EPG9KqQq6+/XDOxnw4ilE/lf3G+dIciGa+HnSO1hg80hbsAnxoDkAPmR2eJzAJHxjExonEAsaNEDcGh4ZIQnOKPkNhWuEGfEw/Xu8isDz2F+VCri6X6FgrdG0hZiPAtFECWENR7kKkivhC8hAtcB1FiouF1e4tWq78xKIBS9VnowBUh6UpOknBithSmSacG02cQYRSTzRrQtVcjQE9ulWhEKCKeq+guOB7NjRatQVWUNIO8g7VUIJ7mblm0lb7MKEehppo1A2sSq413hqBqBYi3rDuAB7EAbtQ458aFTMQ5ig2IEAskqp4LoIKY2grPJ2SIunCwld6fbJXuPcrLhkTqCK1jShJanFvgDKuQ+wV5uUGE1nh6PTX5eK8fcWwmqi57r0W8CgxuRM0buEexcmxjLV6ORgd488oiwOlPUBlnXFfUpub42Y67iN2wGhhQtt8+rAtCLJONlBDxDDa+rfkM4oyfBCwD0cn2GHDAbBGRrkDYPp9oi2p8ZJ6mRkFhIQWDRcgjDHpEiify9Uz65o0haUA3Sd1R7KW7x2hyA/xMYASAU8sBn8EDkb9fi9VP2qoI9Osd4ey0ja2EbDt+m5Wa+uDDlBRBeRCP6i+jgJxrSslbtRooh7CzAzI2cRQoTAghxJQMPtnFxiGW0nFN2LLkRpAyeYWD6lBlE2xCeXGUXXhuKyEQNKHlV78XsMgOXT9kq9qSM8t8Gpl3MyhoYRj5v8OSoGa2fYIKZG9WWzivR8hkVmQJJcLdbA5lVYCXCzCF6+0prrxhOUPebRE26GA9pAGxweoUJyK/caBsbxT0hbSfRlCWGAWqeODs4F/BjfHBsRFJSxZTZfks/DTXbrweHYbStmggqfpJGBMMlTwRkKM0G9mWndYRC6qtMCl8xgaQoWhvChTL6qyqoaqc40JC85bW3QKveH7gBV3ZG3s5jZK2axwYfNTuhddB8RIoWO1VYUknzFAharnyAXiQMqINjRr6BujgDaKsg/UI7WY4nqn7GG6QUTZGplgfsBkZUbiWWa1pggnSxKkkKgDndBzzAUwXncVEefJkdNogL1YrkQ64FXlVwAQilId3Mq2FYiRlh+NDarb5ROfOJBJfxKFBjeVV6vWztfKUY+zrymui4cmANRgKJQqiP50/8UjUfYSyA5VrHKUyuMcJJy2oFi42mnEiuo8oW0hYP9TPhGVKE+bgq9gajYAAdVFW0XDZcl2QhOLR6UARG/PrkqouCoeBY3lot5/rYNEw4RCLjuvFKAahHHkfVDKtLoyLmgMsW4qszKgSRwWg0bpVwIS67yAUcyeog8E9jx919U5MBhmOQ6ccBfuEVT0ET4gsF0aBjiECeBbWc136ryjNhI7oiiozaYBFcin5gpIFCIswpzkFlQrYUzyeTyeD2VSCoPkIUPq1lwD3RsngQdG8m47ck6R+vd9gWQFWdlaqEYiWGnAYau6jeAxcJhtobDNXH0dgIEZ+kPIA73BTPMqA314iGYZmkFWXVsdXkIlXC+C/5La2T45gqTMTMncujoYq8CvND5SmTCLHtEBXmVdwgJAnCfOTyWpcnsY6sOVg5LRMW2RuIHorcEf18Mt4WeXmTFxN2J2MPrfkNYqCfkfaBYNeiGfBMqizQIS/B0RCTkrqmInFUH8llhAh+ODAfh6YG80kQpoVcCEaCAivmiQvSVMY8qlLGS3Dy2FRUwLInHcAxz8BUsSBxhBnCSuK5mypIQdYC8AnShTzDUpW1BrRNRD1iiPiBPCzAAyPm4OP6ij9uPXCRWIS7sAI788xLQuPrXGD4hszNKFBFwr0BuIm8Yq4mPnpqfUYW323lRL8EOWKsNevfK79VkAPG4aPjwJvHqhYWtBDQhyopYO1V2T5VvE4wt7zaHbui+vKIS40imAT0lPghqFsJfgYk8WPg5pIFfRyJ4iB7GqJ+apUcB4kBAhUFcB4V18gM5N2Fq0k4dDrSUMskjZHW4kT0zKYKEkXFmSSbgLXq0dlCxxhmKPPjzauQLkQSAHR4fnLtZmKZDxMwItUkou9j26KSHNzNM5PMTtxOBKAWiGdoS00nKC70xX0dKSAIAIBkZ0A3SYW+h5YVkaQPLpnAS4jN67bKlV4SrQF46mBWZzay/4Sw1YCwu6QRs3A8L1hBDa6IZnxvw0FdIzrxJMkxd5p6FSbQEOh+7JnKzBgqEpNbqEF10Lcmw7MTfHx7wHfyQzoT8MVdtuuYow3cE9LjuVvhnKbpaHA6/i9A41BvuWq2Q+gLD4vaTkDyiO2+Kp1XL4RMW21YKQZZraxyI/G2+Qj4B72hpRSygrk5eQzRCqoI2QNSAfVSsRsF5+IWQLzmla4u2KpngWa0/NNWxih4gir59NTCZx+YerV5JHXYgIqH9DuKo4S4JI0K0EFIiqZBnVeEwrPzD5j6aw5o8oeHzA1ebWaZHMuJxFjjJKdq1a1koVfR3EtOYSwL9h0XiZI4BARp4Y2QgR5ep8RackmqO2bBLcZzLNdUZEsSDEn5hx5QMRzcRSfijuHHzaTwKpiN55eQjvJMQ4oajOZucC2jOvEcHLzUMoZjGuqBQqzdJjbDb2GTVADHVCAJEVbA+yZHk260woWgXt5+d6eq+qvMAuELy3oHs21kKDJuNMmQoeoOEbqJ4QBqL0BTXk+lQj0oTI90cltWV32nw78WlAB6oLZneN0HzLG60fBmC7yWgplcrRfVknSVoiYkRHS8jqjFgqJ1j5aIZwkXQUvk9izP0jBlAbSCJHELWvzjkZoaJwhQFQK1XKLOEzwtqbKPScjvqbu74gothjNAwVRDIAoKEr9a0LMBTVHr1NjfvT+rXFHa7sqUqq7sAV7kg2k3QVChdhwt2qEG+Yyl7qCsFiRVpbhZjKI6s7hYRHWWGhwqRW4AjGNQ8EUSWDhLYHiZ16OglfECIQBwsGSCNH8N8SXs4PLce1/BqfIIbJeWQMmAH5oB2uadhfyHSuBLrEaPambspACYG1V6mvDFVlhC8PBUdFoda5gJLRFjWiBLZFTQeBqYrCf4wGGrh1FKvsATks7Q+EK3RuDpYrsdAwR2H60Q438wUkRku0YsTpUEup+gIUYOan/YT0S/NgmUL7hoxOiDzCJZI9+ohfIONUuE6UG31smBJPX63Q4rq3nfm3oYSXWt4V9IA9t+C8hW9iHXABqLqJXbdQspaA39Kh77rWr9QaeiDQWGmEOQHrSYkOBSufyBikrM12nLRxI2aM7JNPVUhvDVSssxCB8YsE6Aygnd9pr03iqNqqPJKgMDZ25H3GkpESAxtctAGxmRRGIC5ITA18zgTesrauxGsPdO0krMAdsRbs0qZeL79zSvClCvS0usqiKXkmZhhrOEEaJFq2EWtXIc0T+Yz3X9E3GVZ4TMAuLGxaVlnDqWpO1I8HEVaGAN1JOBaFd7q4rHyLCs6rENMgTMYpyPbt3DB0yqYxQMdNfCCs4xcQ2UtCIb6Io/LnS1CTmAIp44K/CiukgHv5teYIIg281F9b2c8cYC3gyojNzgr4CiZIqnemBjEXeoVzrY1yFBSurF0vS8lfxPThugploI5R1921L3qiwT/0FQuwjFilIRC5V61ESnl3jYV8vgajxkxot3kBzSCkOh4scK9pb7MNeISrjxaBoTOafW9qnK24M7aU3uk49BGaidj1kL+kDc0IoS2VXiuHaZwIaZlNfGeGVLBqNxj7nF/UBHfcUkhboewuBn2+ECExOovRv9PbI6X9VQyv1LHvOAejOgpJpYkPA8B9hYqp8AZVybJInJ+YEYRyU28rhF0udARR5Z75kwbAREnl6FWoEJxaLo68BsLC1ZI83xNijg1+mLpInqNRKrzaKtP9ARpGhRPKuI2yrAfW2tzOj0yW+yfhJ/EnavFNMct7YR/QPDpqaRQS5GCC2CHYThxcJnUFF9kj5h3URVKmQQtBk5ifW1Kx8/HfG3ZBlf7WaqV/fpb/SqJ2UFgulrYKzz6YGcoHh8pYcmvtwfofqa3RyqW44qM3eJ10UuoSkMdNVaI2x8tN6R1GhauA9G/qiXPBZ1tZCKr91/HWeT2/gavIC18PpL8HKmNdCp1mJmdVegyCLPSpZFPTrPBeL0isbmWdSe6NQnc2TOShWMMnOXt0zUEQHGRHQiu/EvFK1N1NvTFYvchkQZdu4C8KvpOsjrkyoAIRFM6mNKkOPKDa34q1tdZZYsjc+I1VpVaUxda19HY8jY+XLdQlcDNYxoVQ0Ef514dFlX2LapEM2rtnqytJyiwnxr4u+JHoSrQlYv+rBDimQ1tWmXiBoSu1YzyEicBpZTT1uTFvgh+ujVZK+mFvGD+jOYo67b0bNis3gnGlTFGLA9qoM2YkyzuvzQq6RB9nKnSZ0T0f90h/SD4CrMM0JILXQFCxGGlmoyJhrEXKoBLmZDbQYNjMHdBZRZ0XouJLkIH5Vks7ZXqLGkEVTc9oCOSFvwG+Mh34Y0X5Z0F0AI2tRL2Wt9k5fh88HfflF+3CT6txeTWfbgV+KDVPGdoAmWqa+c1eOGJ75lvYZtcIFL31dEUAMDI/y6OrpCY0MiQjWAwo2IA/42l5JNfanipV60lXERQGIqmDZ81it7tRYZJXXPFMSdh5BeA6Ra2p0uDR7/Z8u7ZN7a5WcvSOlTxv41nsKpvHKpS2yij7Sa+pYoDtwyjZnbAA76hZi+AlVUVzmqmXeRsPqCTLR/1ZIo4T3hre7wYPDF2nAGugK0wR2JbMWHMNpV0+dR83C5aidGpDFZdoAZUr5qsTq8thB3cvVoKWl8dRRvv/gx/hyNpZpcVN2H5NPakRaJsO2BYH/kpf0Jwk2cJSnyiG8Cs6TyEX9iTaeujKWE+CACrDVSnbzAjhL5ZKvaeMe4IB0SWWaUh3agFuGoDkHySMaTSJCgfIV8jWQUCKvDDUnWihoakFgIw8jkr6oNE0xArdrCoKsofO5XUdsFUYb5S1c7kFqGUGCWoNjJb4sMAnBKXQ81jJOvxnSa0wIqikGqZULDBGzh8QB9ZkrroQwnrC44mBA1j4s9UZNul/xjPMGoWVEtjqfXVqqjqgvgBnRoKZ+UF9SY/L6HMedQV6GWTTHqpq5XreNoy8YFPbi14QJim3gHGQopb1MbTQgd9YkKHk/2NWrlAUeqZXVVYmfmc5e2wwmt9tLqKXHURIteXVpnqz+dsRz1ACZB3m6rncMvIhRyUL9R01LhHHWh85YH2CI/16NptcN85rm1Uw+IjG9JUFVJVXybNossTbP2J1U1WTI4tttrWe3ap/IIcDh1MMOkV6ytRZvQiUtktuo28VUojtZiVlUdrJRpWn/CqjK9WjbJSlv0nHcYYlQOzITvkcpCdnLliZDBgfL9wVg1L1uEHDc+qWuVrKi0wKMDj2GKoIojQLNqui/3sbWwjGnNAqBHp+Ol1EY/vtZVkr4yyFc7pN529gjBmVYcGr6/dtwQOYWkOBgVqWo0B+n6NvKkqPY4NEdq7xNCf629ZkuTz+SoBQGLGxzwm4/KVEtbCbVESNzHgZtS7Zj7efVqcG1L3yNBa6zaoETcmcQoNEL8xeaeWfJMEfOu7YvSXJePaln96jySqTKpMi3ywX9d9aPN+NJ7CvlVOy7HyS+/X6I/l+oFjKOuje7aRPLBFnJxRTDeaKsnvGun+dAw+IPW1HIAN4EVFeQbBMGDW1rf3ogpKNEWCbgOtE7cVdKWi4v8VnVQDkRdg1775rAbd7tLBhCN3IVWyrs1ZKYil29HVolbGkh7SwTGU3vjEOVItjRxOR282FlbP5YrMD8yOr4C2xJ9+wL23XiqmkfIctIezpiYh6BOiLWyWqpu0CZJFTCgasSOQ7AZ1LkBHm4uavPr5JGAh1flRBR5reHx4SFCwzJj2rYkF5q09qLdq6D2dFrOi68lW91K6RUtGU4Ve7CPDTdbUWbl0QfqNM6cAlKcV6I6yiZ0klcXlVN7C5Ol25MyJg8vSh01ok2p+3USqTsFVwwvTK/dsUVnAcTX+XLi1x98gsN99Af2gcF9InPNn10t5CdJKLGw+sQewBJnqsGdQNEuHlSuBM0oY7Xqjo220OBVKxoMC+MYvJq4zWupOqmowMCrdUWtFJEUAF/x4BnE2TCgugpxHK5GLY9pP2HgXl5jHJlJRGlTuKkRtxDHkrEI35C1l8b0qXf59ygkCV4pN7dw6lrnrkX7Uvb7B/9drT8yOGI2BDDwKJcEkMA6qrWoxRJ4ZMLORUG/1irZNC2R2uUXS6Uw9TtNhKz6udV+hqnF1sjuESsefCFJ0MFaIsWaAScYNaeGOtNk2chqgeUpC+PJsGu+s2op2tzS0WrkqrA9ptJNFUdyOeukjeHVNmSvklwuElHNu4sXXG2KNy9xqOM+1N40pk6Z6NF37bgCKdSf1c8frdzHyThvcmxszEdqjayF/b+FwaOdKtKJvYz8hCMMJzvxypqoOWRtHbg3lIDjSYD38sYftVDU0LNeB/LXlF7kln7UTIZSVFXRWn9FH8Tw2gQWvjswa5DHUad1xMYz0zwDg6LNK9DVeq3+ujPES1LPA3FharHRdmO0cdJCNE8UCMirlpoLAKJyAMJe1A7ZkRvwK/ZwQJrDD3UzJBXdNpg3tzbkYDXwj1eL2vM4Bt+jq3xVzxM0VrVs+7ae1bdzeJ8av2JYTF+hOmmN5KA4oAL1DKic8OJIrfNeG62TGkr2WwgUZs7C5AK6PFvSZtNGpNxODHKr7cmbNFR7r6oEJpd4K8rxisLuUquUtuXkoBbBHRL8w/wwkHycMADaRYm8rhtAA46EDsD26Z0KmfPieInKgRBR62qfIWA7Fuq31Ig+lyM+oEYylSOzuuZQK+pfZxo07fU67T8oyFHVydWuuNXuqSUi8G0dtbT1jJacUw2uJUr0dixJVk00a08FGke54AzRuy1UgphbFlMZyhEFzdOpCUDrbkfFla6V2aV1EzytV6fNWlZ190gYlTTgqaTjOk7TthGMu92itQigH1J718P3ziQdQVRO0szeZZjEoH7G3VFo2gmFVOgHLp4G1mntQl1PES8zXlcEugDtdrWqhgtRp9NSoV9sDvSLgFU3qCRtUSsv8oS5UDO59jZ49aUmUnn5xuePihR7Kyo6LkQbEI08LehQ3LiavhBhrlaYyrSSqPAH4RArQkAcO19ihAVT6N/ekND/1qjqe+EZB+he1et31RbOdOhXkIp2sOGcrKv6u9+iOECE9CNJ1Ml3tW1xqPuAD2soVPAeERFeP/IjZC1wRJJenpbRUdt27wjOqO1owHiOBFB4q/iloHam1M7Vvh7gww0pV75Dp6nsvrRlImsfGPF8Huv0sLx+Db/qXALUv4bhajUDkeX1d+/RDRV1x3udDoYpUetXa75B0Y6KCDKRfjFqE1XXVtDdX0mppq+rFYAqicGWAkNZL8hiJEQT88tMw26wCwMfUF7cHTazaTdxKpoCgDhOr62uHQAFY6VGOimm/aEw10NE5A8ENtXwj9VBHSIL1v56yYAC09EK2iOaGqbsfoAjUEJEIDS0IY2sgD11+MCtfARTSrCPII2KUEakAIbzp6qDcZOkhXlgUTJJfSO7qRN0qRPorfSowOwn4JVBCGknwpw5nxFtpNoV6ah2iVdv15ZGrVqsc4YrV+cGLDVEJq1QSFFrLQ1yVIeoKkQd57C9TgrQ1OZAKGgba0HyIBcDIh6j6rLK26d+Ow61Uz+oEf1tuS8V8t+Xa/s9k3LCtDYxCS2iDGi7qlcu4bzUyDjeGsqceeXhmfiuMB8Dzg1vY3OSbIXroLylpbihvdQn+K8thsF4CunVIWOJZD4or+6tVfgfHgTw0T4KLqTKpOmkmbW0patnddrPx1HAKkyTEsof54kODGp2Qg9/xUEQ03BjUW1YxnPXsWdbamhVwfDhb9MrEPt5PQkH+L+uDa3Jg3i1a7v91q77rZ3eB6DXfpWOIyPcg/YBHi9xAy8lYT6WX1uc4tHpHmTHsJq1L0gi/cUKYQ+tTG0EaZqjC6PU+LVnaMeHtsHpxJ4/YMX9FV/4S+dPvA1nj0rxKLe83Q5QvzdtW8I2PpTQ9jydd0J46CAl8ozEsqP2HCXf1Ebbt5nxWlH7jZ5EOyyQtQSAeg1D0P7UPaRfUY7atsFgN/AQupKX1S4X7UPGejRit95Z3yonyQ+Ag8NfR8srwZ77Zq0U9ZeBnNWBg9oB8Ads/IgnRAKG4+JNIgmskxP0xHAcxP6WZvB2zGPz8TRSFFmTYlgZE7K0SQxHd4Ok81Il76yAmwgZ2Rd13kbA7GjvTdJO82dyoJuIuyIKHZgKXM8u8kC9XhXBtLWf+JBQesGn3V9qTAAvVEBvF9OAF4dfO5peffyNXMt1vUbMjUrqHptbAAxkutqKGJqozazoSnRcGDIi3EAdcO1O2AciUSciEK1OW9x1AglXhMuC5VOykLBr1bfzdFObsXFaqnJ6LaZhTtBc9jam4AHValV8d7voRBq1hoYZ4iYavpN+LvSmnjCdINBfGky1Mm8tqsQyZLDWawWaipHSXMIHAzhc0MM47fXRItAJAEh/ZS3XRO2k1PYTXIMsNz+WecNpqlKJ5677Nvf24mjICNqtLlOVoNHi2Ims00fECKng6QAZMyn1Kg0Y8A2eOUOMWFuYTqciaWcSwzs5AUo68t+qxZZK+GtnMoLYz7p+TtPQVoOsFVkZl0zIXdX9m5s60iRob8BVI3FRiVrVy6aTHXTMiybBg6YEgs4h0AEmpmI+vgy9qa3gB7WCzSJ6eJ1Oo+AF9WhNH4fVsgyiQkbHHB3b6ruWYJLJeuv2+iy9lneqecBhkDdCxV904VXBTo4e19Z1CoXy+23nQ3qqZqzdeyfrRrUtR5vp8tu37YeOwNC5R9AUtzqfx1BT7kyZ683e1KzQtYMAMkwfUcqgJfI7vpXR9tno6uZ7GO22J8a1qRpRdbSc75NqKiqAYzl1UIhal79TGXZ744B2StpmnFXqc2oAKh2Wz1ixphYSLWYunWaUOmFetFyDf/2eTR2+OnlgRjQk6azuqylBk52+wdDozAvGQysOMoISEw2fiJDX8Ttyr4RDQMaFrd2pCuz54aoCi6/uO7AIjba094nRjQQVEHekKEtXfYSk2XD/0s5YLqdid/7OdZgoD6IJv4z0U3EBNZrR3obv4nf4YWLwbYoAJ9bbWsP/dCRCV7X/lZbBHVKXtxRcesZlwxY+qrYAIELT+NH7duGo5Nt4eAtdpxNiQxEUQk11FRccdj3MHxqroIMudERav646rZwUmYvZkVV3HZ1egQVaOpLlFfy+bdEJzX51stdVblWpZaxmHQ5kIp6a1lNReeCSAvZiIflYnggfZNqvrSZt9VlgpHXSkPYm7/PtAoRY96quISD2eRvniFAmk3BSTQnV0bQV7rzt/K8LWS2JUJx2U+gsJQRCO6E9gE7F5SmdTprin48EOfcQq4640EEEG/ZtWlvcWntQ4c+qv3D4SqhaTclGgjSi3GmzmPpyvtN9Mg57m444mZj9FgusZNDVQUuWnvOWenuqAims8c9yC3guc2pz12ZKtK+2yGpHFpmq8ijWI2tLkxqii6rm6SUGOsiitD3gkV5KqjdhOy0ujPohQtD5DC/g1/ceDyopM75vs3Zl8m0J30+1K1Kf9K6gxUyvS+iTtEbdMcDqoozoLvVuoHVP1i6bprbmqOpAnSA5OryhELpOIdFBNU5mKfEqFdgfW9f89816f3zVsXD2sz2/6Ryv+TycpKfDhMogERkbFfn2+C0dEqTtc0AXpCodjwJTg7QOckOxVm29BnbB2APK6WAMzDGcdQEJnXACvs9UT9NefjVH5AVutYy6hdEU1KS/QoeZVn+19iFgbre26qohDjzzFT+hDWf5+ZSEe9Rz60QwNYWFJxnAgXecwCvlpe8YE21tUUG2Bhe1mWAP066QKYb4Eb3piV4YnxlTCRr2Rnz065mMoW2LOvlOdVwdLgREOQ2OSBl5FnQsjBoeARzsPRIS6cujXbwgtpzJUp2fCDWvikpGx46Ss9rA8yDXELsfs3zxpfbno5AUjD581UEw548XvAD8M3kRftucXrm/8FMl4r1//n7/d4H26/06iilqmyboqfZLLOt67p7sV2Em6tgKq7W85u3Y0EBKADOdMKiDRkDorDWmqM1fOmMMtSy4xgyMn/Pl3H95Ip3Kgo8MVQtQnVRbVNvbospXbVFdKiu5+6oOpm0OAS+GBX1dfkEdFMzd6jrOBvGvlQTTlnt1G70Ff9WLrs5SQOZbdPpSBWwAOw94gHxAqzFKOmfrjK+kqY3T2ogFfEiuYK2UdSAwHNWhybOdNr3C+AzRficKAnxDx9xVVY7V/yE+1jkOODAwF5PU4zvRUfsvtLsofPrWae2GQFG5LupowQvyICgA9nK1hVYhoMaYrH4k3B8KWS14qvAlHYDDzR31CbkUl9rEvY7TAhWPygEH/7vU2UDeVJ1ppOphaTplwnbXPlvCHM2lPkuAFN5a5tZRpwixq2JWMG0yfVGkencu/Qupd7RTfOGHSdQ+1S/gdIak4I8oc4o3FY5AGx2Bg3x9u7zeehtMkXnfytrj1BKy4ashvIN5ak7oVjASl750uAsSxetoAlO8VXXVqYtJG8tGiWr3YMRhGEN4EsbIghqX1mp0rs2aaghH7iURZNlFZ50dhlcrmYhgnWowhk7xkdhXi7fXmhbCWmtVQT2nawmXQIGq7TNIH6cdr1UrQl6t4toOiNJ49fyjjj20Z1YRn89Xq1FW8UcHLqFM5uvlklma3/lHvSNNdQ6I1s+JFUtqVZE2bqp64WmErm8TYproMMxq0b6d1ZJ2ZzB4i8d2H973f8T7/+prsrewvMn+pcKD4khzXE3BDFh2dCFCP+mUtaNmG7ChfscRNp2J1XTorHZmJgxF7MdZnN/xYEFrZ9reiHtF/HntqCS9iqraWnnQAkbHVuBM0ARqiQaaX48r6BOmazgOInrfrBMiX50d8EraLdqIxd8XSTpRaWJytKNEW4vz7OrY6K9/LmS3H72don3p2uw81KeQpU9JuKFlHN4CjBC32r6mlVGoO+iAwKPtEZWAQK8uHg0tclCh+LZo2vz1HBtM//VDAdfjjV/Iasv6x2PH3HfuGIBn3rv/Ay/kNQPiOR6XAAABg2lDQ1BJQ0MgcHJvZmlsZQAAeJx9kT1Iw0AcxV9TpSJVQTuIOGSoThZERRy1CkWoEGqFVh1MLv2CJg1Jiouj4Fpw8GOx6uDirKuDqyAIfoC4uDopukiJ/0sKLWI8OO7Hu3uPu3eAUC8zzeoYBzTdNlOJuJjJroqhV4QgIIhe9MvMMuYkKQnf8XWPAF/vYjzL/9yfo0fNWQwIiMSzzDBt4g3i6U3b4LxPHGFFWSU+Jx4z6YLEj1xXPH7jXHBZ4JkRM52aJ44Qi4U2VtqYFU2NeIo4qmo65QsZj1XOW5y1cpU178lfGM7pK8tcpzmMBBaxBAkiFFRRQhk2YrTqpFhI0X7cxz/k+iVyKeQqgZFjARVokF0/+B/87tbKT054SeE40PniOB8jQGgXaNQc5/vYcRonQPAZuNJb/kodmPkkvdbSokdA3zZwcd3SlD3gcgcYfDJkU3alIE0hnwfez+ibssDALdC95vXW3MfpA5CmrpI3wMEhMFqg7HWfd3e19/bvmWZ/P9VxcmgIfogoAAAABmJLR0QAKQBEAP9vAzybAAAACXBIWXMAAC4iAAAuIgGq4t2SAAAAB3RJTUUH4wcCAxIbtPrQcQAAABl0RVh0Q29tbWVudABDcmVhdGVkIHdpdGggR0lNUFeBDhcAABpESURBVHja7Zx5eFRVmv8/99aaqiSVSlKVUEkISVgCSUAgoLaCqC0I0ihEUITutm3tZgAHoRse20dpe3Br5TdOo2OLv3FDUBEH5ueC2iPSiEFodjKBJEiALJV9rVSlqu6te+ePqlQqGwQFnXl+/T7PeXLvqXPP8r3vebfz3sDf6e/0d7oMJFyBPtNDZTSQBVgAW2isbcB/AJ7/H4CZCPwEuH7hwoU52dnZyZmZmcTHx5OSkoLL5eLIkSNce+217N+/n7feeqv+4MGD7wC/B9r+pwKj/ZbP2YH7c3Jyfnrvvfdm5+fnk5WVhVbbtzun00l6ejoWiwWPx8Mbb7xhLysrW/Hwww/fVVpaumDNmjV7+xugoqKCtLQ0BOHbv7v29nZefvnl74VjxgO/WbFixaK5c+eSkZGBTqe74AOVlZW8//77zJ8/n71797J3715WrFiBw+HgN7/5jeejjz669dChQ3urq6upr6+nrKwMRVEoKipCp9OxZMkSpk6dSmxs7CUvrqGhAbvdfkWBsQFPrF69+leLFi3CbrdTU1PDyZMn+fzzzzGbzURFRTF06FDmz58ffkiWZSoqKvjDH/6AJEmMHz8eq9XK2rVrKS0tRVVV1qxZ0/nBBx+MO3r06GmbzYYoinz88ceMGTMGh8NBcXExn376KVdddRULFiwgJibmewFmUFvp1ltv3b9y5crMvLy8cN3q1at5/vnnKVi8mKhQ3fPPP4+iKFRUVHD48GG2bt2K1WqloKCAmJgY2tvbaWpqYt26dZjNZkRR5Omnn44qKSn5fPz48aOdTqcHwOv1oigKlZWV/PnPf+aPf/wjra2tLF26lGeeeYaUlJQrLmPEQbSJzs3NTY4EBWD69OmMHTuWQETd1q1bWbt2LXv27GHv3r3k5ORw1113hd9ybGwsGRkZZGZmsm3bNiorK7FarWzcuHEo8HpXP7m5ubz33ntIksTixYtZtWoVdXV1rFu3jpdeeomampr/EcB0rF+//pXKyspwhSRJREVF4QFMgD9UP2rUKO6//370ej1WqxVJknA6nQiCQGxsLG1tbdTV1QGQkJDAhg0bKC8vZ9SoUaxfv36Bw+G4s7S0lKKiIs6ePctrr73Gjh07sNlsbNiwAYPBwJIlS1i7di2NjY0/ODAAG7ds2YKqqgCUl5djt9sxAW5AD1RXV5OVlUVjYyMHDhzAYrEQHR1NY2Mjzc3NiKKIxWLh1KlT7Nu3D1mWmTFjBs8++yz19fUsWLCAOXPmbLzxxhsTXS4XCxcuZNasWdx+++1MmTKF5cuXoygKWq2W1atX8+ijj15RYDSDbPeELMv5R44coaioiNdee41FixYRl5JCdIhj9u3di9/vp7CwkOuuu45du3ZhNpsxGAxMmjQJSZJ4++23Wbp0KWazmS+//JLk5GSmTp1KRkYGWq2W0aNHR73++uupixYt2h6p+g0GA4FAgHfeeYeEhARSU1NxOBzs2rWLCRMmDDhpj8fDc889d8WAeeXxxx//1bJly8jLy2PixIm43W6mTZuGPS6OdkDyeFj10EOMHDmSr7/+mrFjx2Kz2bBYLEyZMiUo5bVa9Ho9ra2t5OXlkZ+fT3R0NGlpaZjNZgDi4+NJTk7OW758+WcLFy6sstvt7N+/H5fLRXx8PA6Hg7fffpukpCRyc3PZu3cvycnJJCQkXHZgLraVlq5ateqBWbNmodfr0Wq1fPnll3z99dcMGzaMDiAWKDpyhGXLlnHnnXciyzJGo5Hhw4djNptxOp3hLZiTk0N6ejoAPp+PV155pY8dNGfOHMaNG/e4KIrU1NSQn5+Poii8++671NfXM3v2bLZv347b7eZnP/sZL774IrIsf68yZuL8+fP/uaCggOrqajZt2sSWLVsA+O1vf4sKRIcavvfee0ycODFoAY4fj9frRZIkNBpNeOsdPHgQURTRarXIssz27dtZuXIl0dHRPQaNiYlh3rx5MwoKCkwxMTFs2bKFWbNmsXz58rBQ79qOWq2Wu+++m23btn1vMkYLfLBq1aq0999/H4PBwJw5c7j99ts5ceIEycnJuNrb6ejo4Pjx45jNZkaMGBFEWhQ5e/YsVqsVk8mEw+EgOzsbq9VKZ2cnbrcbk8nE7NmziYuLG9CU3759u//ee+/d4/V60el0OBwOUlNTaWhoIDs7G1EMvlOr1cqnn35KXl4eJpPpim+l5evXr5+we/duVqxYwT333ENqaioAM2fOJBAIUFtby65duygsLGT69OndRk90NE6ns0dnLpeL5uZmnE4np0+fRhAEGhoaaG9v73fwyZMnA8wBuP7668MG3YkTJ2hoaOjjk915551s3779e3Eir7vjjju45557+vwQHx9PfHw8QL8awW63EwgEetRt2rSJv3EEc3o05uEx/PPrLxDwKHhrPAS+8nH99dcHjcVAgPT0dDIzM1myZMnEGTNmJH/88ce1brcbj8dDIBCgoKCgz5iJiYm0tLQgSdJFfbfvAkzKI488cmdvthwsxcTEcP78+R51CxYsoOivJWQ/kIeqU1EMKoICgk9EkAR8LQH2tx1HlAT21R/B9Vk753efAfjwtttu+3fg34qLixt7W9+9X1hZWRk5OTkXC5GkA7nAuND1L4CiwQBz38033/ytkTaZTERayQA2m427bQV8cXgfCZPtCAqoooqgUSEAmmgNWr0WwSdgjDURlxbPsKsz6Khw5Tefbs4v/2vp0zk5OYXAJmBzl08VSfn5+XzyySc9gLHb7ROBm4FbgLHitXp7TJYFyygrxjgjUq3E2SdP/ScwGai4mHd9qry8PNtoNH5rcFasWMEdd9zRo87n87FuwzoSnk5FiNGg6lUEWUDwC4g+AUESEHzd96I/eC34NYg+gQ6nC+eJSir3lDcArwCbnE5nWeQYDocD4DpgJrCAPHFE0g2pJFxlIyopCm2UHiEAKIAqoGkTqfzbWSqe/+YAcFNkZLE3MEm/+93vah988MGLLl6SJJqamhAEAZPJ1CMc8Oqrr5KYmNgnhlJcXMwb3rdJvWMYqkFFhW4AfGLwOgIkTde9XwwC5heQWv00nW3k1FdFyC3+L4E1Ie16F3C3cJXOPnRWJrEZFsyp0QiKEARCIXwtKIAiILpFAvEBTn9aQsPLVU8Cjw60lX48bty4Aa3Io0ePcuDAAdavX8/Sxdeg02pwe/wcK1d56aWXwppr5MiRlJaW9gFm9OjRGNeK+G/wo0vUg15FFYO6URBVVE1oiQEQRFBFATQgaIJtVBF0ZgOOUak4hqXRcL5m6pEPDuzHBuk/H0VCTiIGaxSCGurDFQJF7QaFQPBaVECQBQKSQMotQ2l4uerXwD91+cS9gZk0dOjQHhXV1dV8/vnnHPjrDu6+bTRL7xjOk79+oUeb0+fqeezpp9mwYQMajYb09HR2795NVlZWT9tAFJl24zR27tnF0LkZqCqgUUEDqlYITjoEjqoFAqHfRAVEEUEjgFZFDQgIIljsVhgmcM26G9HoNEFu6AhxRSC4XcQQt6AIIU4JgSSJoFMQJQFjgpGYu+MTXe82zwoF6/sAM7JLFcuyzNatWzn+1Wb+8b7pPFSwGDEUfy0ua6GsXCZ/nI60IXGMGGZnYqaX0tJSxowZQ2JiItXV1X247vTp02yt3IF7XxvJ01LQJepRtWqYY9AQ5BoRBFFAEUHs4hgtqAEVRAFBVOlo78BZUgXnVHQ+LaJXCC5eDQERiABCFbu3UyAEnCygRIPgDyBIAknTUnC927y4X2CmTJkyoYv9N27cyPC4s7z8x/vQabrtwNr6Dt7cepZYo559B/2s/PUIHPYYZv/4Krbt3s2YMWMwmUx97IlAIMC7O94lZ804zv7lG4qeOUzKbenokvWYEk3otDp0egNCaGspagCp3Y/UKhFoCyDIAp6aDvxtPmpPOolzxOPISCGuIQHZKWGMMSIoYo/tEtxCQW6JlC1dgKl6AVEKyjTLMAvAj/uTMclTp05NAigtLaXk0A7WvvgPfd76//vLWQKKhM4cw8F9hax+opqXnprOsDQb27a9zLJly+iyWNva2rBYLABUVVXROtZDvEnL8Pmj8EvD6Khz4TrfTvOhBnz1PjwftoajXoZrzBhjjcQNTUCQBfQ6HSZHNPHDEsi8dgQ6dAh+AVeDC3+NDzPRIe4IARGIAEIFv9uLu9WNu6UD2S+TnDkEndkYFPR+AW2sDuOcWIv3g/aJwOFIYEZ1md7l5eU8dP+tfUCpa2intMyFxqCntqmJU9+cJjomlr9+Xc3tt2QybUJcGIzMzEyOHTsWBqa5uZloR7fm0kbrsVjjiR1rRdWpQc30YC/VHZq0Jqy6uzVUF4B6gw65UUaMEcOcgAq+jk5a6lppqWmk4mg5yNQBJ4CjwAjZJ8/NTB4Z7DM0TnxuPM4P2q/uA0yX4C0uLua2e/pG1wOKQpReg2Aw0NbSQcqQIcz48VTKzkhwC0zITQ8D43A42LNnT/hZt9uN3mG4qBmgXuKZjjkuGldDO0IaSD4/9efrOHviDO6KtibgQ6AQ+BSoijwtPfNVydz0yRkIsSKCX0GQBKLsZkIWcQ8nMq1Lvry/bRtWi7nPJOJiTXS4PZyvquBkcTE/yp/MmDE5tDa30NjiwmDU09DQEJyw2UxbW/dBo6IoeBo8F7Qshf7c2pCWHQgtY3QUZ0+doajwKF/86yeu//royGvuirZbgSH19fW/AP6tFygA54GPmyubg/ZRiGOiEswAeb2nYe8y0rIc/c/CFKVn4oQEDDotjQ1NJCcnIXdKWOPi8PsDCALhk0Or1UpFRbeVnZKSwrkNJRx6ppCm4/VIbn8fAFRCcqHXzMQuVooASAkotDe1U36kDL+zs955oGIlkPnhhx/+8tChQ5+pqipdhNlebTxXF95GgiSgETUAut7CNy82NhZZlhk7Om3A3iaMTeZosRudQc/QFAftbW3oNHp0Og2pyVY6IqJpXTGTsrIytm/fziOPPEJCQgKlpaXs/PNO6kY0kXBtEqahJqLiTWgELXq9Dp1W32NMBdCIAr4OH976TupP1lK17yx4aAslDTzndDr/BaC2tpb6+vrB7MIvqg6eC2TfnKsRJRAkAa1WE34FkcDERUVFIQgCxWXVA/aWNcyKx9PE1OuuI2N4FmdOluCX/FhiTBj0OkqqqrriKdhsNnbs2IHBYGDlypUYDEEZk5ycjMPhwOv1cs3ka6isrOTk7pMoisLGjRsvtJh64CSwE3jH6XRWORyOecATDodjGnDvmTNnGk+dOjUYYNqAfe4m95SoOBMav4I+xtA3UDVz5syhAKqqkjMyBb8cQFaUPr1FGXTMvmUE06ZOxSv58XV6yB6hQ6/ToNFqUELPVFVVUVhYyLhx45g1axaJiYmkpqaGg0yCIOBwOJg8eTIFBQU89thj/P73v8fpdIZLdXU1DzzwgAIIW7ZsEebNm5d0/PjxG51O53NOp7MqlDSwHZgEnAOKs7Ky5vl8vsHK7pO+1s7gdpIE8Kh9gUlLSzMDdHR0BNWgVoNW7N7ohYdreOqFIvYfrWXi2GQqzp5HURQMRjPjxwVlU3JiLGfOnKG5uZkVK1YwcuRIMjMzEUWRvLw8Ro4cSUZGRlgYX8yDFwQBQRDErlhzYmIira2t/WVUuJ1O53LgXuDF++6774lBAlPkbu4Iyxm5WeoLTFek3e12kzok6BYoajeC1c4mPvt4L6++XkqUKY7hmSK1Tif2JDepyZbwQurr63n88cd56qmnwiCoqorX60VVVTo7O8Ntu64vGHuNeDlmsxmXyzVgW6fT+QlwLbBYEIS1gwDmVHNNc9Crl0TwdYv3LhmTExmxS7YFF1p4qIb4OAs5I8xEmzXYh8YxZYoZk1Eib5SVPQfauHlKUo+R3nnnHTZt2sSoUaPCxyaqqnL8+HEMBgMeT1BlWywWRFHE6/WG1XogEKCtrQ1BEFBVlbi4OCwWC4sWLZoY8nqLLhZEczqd5x0Oxw3AIbvdfjRkywxEdS21jeG4kCCIfYDRRcZTuhil06vS0eEBzMy6aTTTbxhFm8uDQaclM01LekoUmog36vH6WbhwIdOmTetrHIYC6BUVFdTV1VFaWkphYSEPPvggXSNrBAFDyMdSVBW/LKOqKjFwSAK8wOzZs5tC8uRkqOoQcMTpdB6KyMQ4/9xzz/0M2AJc1Ts6Fxkikpv9qiKpgiALBDolgPZIYLSRwCgho2H6lBS8vu59V9fYyYmSNiaNhURrdA9QAN77YD8/+tGcsIA1GAxUVFRQUlLCZ599hqu2lgSTCbPRiF6rZVJKSthjHyzJspwgKUqCT5ImSoEAXp/vgWafD4fD4QI+D4U/d6qq+okgCK+H7qddoMsjfk/nRFeTiyPv7leAVyOBUSOBiZys0dDtJe/cdZ5JVw2hvcNLorXnQVlVbQt/2PCfvPDCHFRV5fDhw+zcuZO28+exx8aSajSiCQWyvlP0XqtFC0Tpu22dtCBgMR1e79za1ta5bdAoCMJLwDMhl6AA+PcBumytKq/izK5THmAu8JdI4RulRgjagKL2edrt8VNU0spXB9pITbb2+X3HJ4d4fMV0vvnmG5YuXcovCwqwdHSQlZxMjMnUh7suN2m1WuKio8lOTeWqpKTEZINhbWibfQWsu8CjxWd2nToNjO0CJRKYTJvNFm7Z1OzqAQiA2aRnfK6FKKOCu7OnOS/JAf7js8MMsVvY8Kc/cfqrr8hMSrpsZzyXSgadjnSbjTybbShwD8HU2vyBYveh3870d3xijDxDNhqDC/J0+jGb9CiqiigI/GJB8Gii09fTDTlX0cBXh6s5cHgbE5KT+83e/CHIZDCQZ7MZioKO7YwQB/V7KjzgEW1k2qjZZAw7jV0yp76pPRyT8flkPBFcU1RaRX5iIvGiSHVLC4F+LOYfglRVpbKxkTSdHmDKZUsDcdZ3W5ldYQifJHO0rBM5Qg7t2lvMsMREbh4zhiyrlUNOJ00u1w8KkNfv50R1NUPjLFwzPIuQ4XfJR7TJGk134kNnZ9AEdNjjkOQAOq0GXdDzZKgjnrhYL7FmQ9je2LztEPNyc4MRoMREbLGxVDY3U+R0EqvVEh8TQ3RUVA8X40pRQFGoa22l0uPhxrRUHKGMCgvEtkE2UHIpwHR0uQRGo5HSM7XhBidKapiYm4ocUNh7qIkbr7YRG93t49TUt2HT63u4D0adjhFJSWTa7bR5PFS1tHAqlAERZzQSbTRiMhjCxtx3JVmW8fj9tHk8OL1eJiQkcHV6OjqNBlUNxhEsBj1tPn/spXKMq0vGWCwWypusFB46zXX5IxibPQS/HECv1RBjlun0SURF2DYCoNdqCahqvwNYTCYsJhM5KSl4/X5cXi+NHR2cq6ujFYgB9BpND7skrF30erRdsk8QQA2eXrq9Xtw+H25VRQKMQEashcz4eK6OiUEXEv4BVQ3LilAQKgP426UA0+R0OsNpHevXr+eZZ56hqOQLCmZNwhYfNP7yc4f00EiNLUFPPNpo7MExfeKVoZ/0eh2JOh2JsTFkO4agqCoenx8pEMAvyz3aqoDH6w0CLgQFaciXISkxAa0gYtLrMer1fb436JqLEDxJCZsUwNlL5ZhzZ86c6ZHK8eSTT3Ls2DFW/8tb5Ka0snjedSTbLD24RafV4vfLKKoanIzQM6IthPwuISIUp0Ysvkul9qiIRGcQ3w+oIe0j9BNIj+xVFVQicLoodfVjNJlM5woLC5OSkpL6NDpx4gRvvfUW12f7uOWGPNJCYQkAZ10rP/nJ/yGr13NCv1F/4TueCVx8IQPRjpISLmWwyIbp2dnZe9588830tLT+Y77FxcXs3LkTWg9y603jyB2Zgl8KMGPms4xyOBDCC1UHOd1vD8KlQKqi8kFpmS8kjr4V0CnA6xs3brxl+vTp4Rhtb2psbKS4uJj9+/ez57O3cH7TxqTMrAguURAQrwggF5JhAzXy+PzsOnfu8AXcgkFz4JIpU6b808MPP2wbP378BTtoaGhg3LhxzBw+/IrZJuJAN0roXrmwqVrb0sbhhobXgF9ejq2ZCDy6ePHiJT/96U8NF8p/czgczOiV8nE52ONybcSymlrK3e5FwNuDfeZCKfMe4NMTJ05s2rx5s76pqSnfbrcLdru9j3qsra2l+dxZREEMaon+StAM6f9vr7b0+jv4IqCi9rj2SjLHGhs7gfsA6XIAE3n+svPYsWObN2/erB4+fHiEXq83dWVpd0X8v/j4I8wGQzcIvSc9AAhqb7XaWxUPCoSu+77tzjc00CJJvwO+vJxarj/SEcwjKbjpppvmzJ0712a327n/rrvIzxg2aC0sDGpqA3U0OK0nBQLsrahwhyzehisNTPBIsL6+63vDmQSzuH8+zmaLijObBjFgb5V++eyZyHEqm5v5pr39koTuZQEmMuonCMKPgMJrUlPQajT9vv2eRt/lB6RLQQH4ZZkD1dUegh515XfShN8xKLQPeOJUVTUBRQnKFFUNleC9EirBazV0r363QvCvikogJHVkJcCxYA7gqm8DymUFJgTOY23wSkl1NbKiEIgA49suOHzdT1EJAR/RVlYUSqqd+OD/1tfXb7wsttNlomUtirrxZFUVshLou1AGWSLaqhGAqRGlN5hSIEBJVTUtivKKqqq/upK+14C0ZMmSAb+ef/bZZwH+pIN/HGVLxGw0XlzWqhcw79WLKC0VfLLEf9XWIcOf1qxZ8xB8v//C4FJpDvCvw2LMqUlx1r7gXEg7D0Yuq9Dp99HY7sLp9XqApcCb34e3fjkoHdgSrxGvy0iyo9VoL84xAznoapdT6KPD56Wm3YU3mE+3AXjrUm2VHxoYAAPBPP01w+MsGHU6ovR6REHsu/B+to8sy7j9flrcHdT5/ABOYDuwleBJ4/fjyV9Bugb4OcEMqIlaINFgQCMKaIMxWWQlgICAT5bwSjKdgBz0274ADgJvMHD2wv9aYHrTmBBYdmAIwY/+a4GakMN3GmgmmPLxvdJ/A1+6rARNPQRRAAAAAElFTkSuQmCC";
                }
            }

            public override string ImageType
            {
                get
                {
                    return "png";
                }
            }
        }

        private static string HeaderTimeStamp
        {
            get
            {
                return DateTime.Now.ToString(HeaderDateFormatstring).Replace("[", "").Replace("]", "");
            }
        }

        private static string HeaderDateFormatstring = "[yyyy-MM-dd] [HH:mm:ss.ffff]";

        private static string defaultParentFolder = default(string);
		internal static string DEFAULT_PARENT_FOLDER// = Foundation.UserHomeFolder + Path.DirectorySeparatorChar + "Documents" + Path.DirectorySeparatorChar + "Test Results";
		{
			get
			{
				if (defaultParentFolder == default(string))
				{
					string rootShortName = Path.DirectorySeparatorChar + "Test Results";
					string rootPath = IoUtilities.UserHomeFolder + Path.DirectorySeparatorChar + "Documents";

					if (Directory.Exists(rootPath))
					{
						// Deliberate NO-OP
					}
					else
					{
						rootPath = IoUtilities.UserHomeFolder;
					}

					defaultParentFolder = rootPath + rootShortName;
				}

				return defaultParentFolder;
			}
		}

		public static TestPriority TestPriorityFromString(String statusString)
		{
			if (statusString.ToUpper().StartsWith("N"))
				return TestPriority.Normal;
			if (statusString.ToUpper().StartsWith("A"))
				return TestPriority.Ancillary;
			if (statusString.ToUpper().StartsWith("C"))
				return TestPriority.Critical;
			return TestPriority.HappyPath;
		}

		public static TestStatus TestStatusFromString(String statusString)
		{
			if (statusString.ToUpper().StartsWith("P"))
				return TestStatus.Pass;
			if (statusString.ToUpper().StartsWith("F"))
				return TestStatus.Fail;
			return TestStatus.Inconclusive;
		}
	}
}

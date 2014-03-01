SolJSON
==========

Unity3D를 위한 가벼운 JSON 라이브러리


특징
----------

* Unity3D 완벽 호환
* 1500줄 정도의 적은 코드
* Object Mapping
* JsonObject를 통한 JSON String 생성

사용법
----------

코드를 모두 다운 받은뒤에 Unity3D 프로젝트에 Assets폴더에 넣고 사용하시면 됩니다.
Object의 Property만 인식하게 되어 있습니다.

예제
----------

JSON문자열을 객체로

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	namespace SolJSONExample
	{
		class Person
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

		class Company
		{
			public Person CEO { get; set; }
			public List<Person> Staffs { get; set; }
		}
		class Program
		{
			static void Main(string[] args)
			{
				// 단일 객체 Mapping 예제
				{
					string person_info_str = "{\"Name\":\"홍길동\", \"Age\": 13}";
					Person person_info = SolJSON.Convert.JsonConverter.ToObject<Person>(person_info_str);

					Console.WriteLine("Name = " + person_info.Name);
					Console.WriteLine("Age = " + person_info.Age);
				}

				// 배열 Mapping 예제
				{
					string person_infos_str = "[{\"Name\":\"홍길동\", \"Age\": 13},{\"Name\":\"영희\", \"Age\": 20}]";
					List<Person> person_infos = SolJSON.Convert.JsonConverter.ToObject<List<Person>>(person_infos_str);

					foreach (var person_info in person_infos)
					{
						Console.WriteLine("Name = " + person_info.Name);
						Console.WriteLine("Age = " + person_info.Age);
					}
				}
				// 복합 객체 Maaping 예제
				{
					string company_str = "{\"CEO\":{\"Name\":\"홍길동\", \"Age\": 13}, \"Staffs\": [{\"Name\":\"철수\", \"Age\": 25}, {\"Name\":\"영희\", \"Age\": 20}]}";
					Company company = SolJSON.Convert.JsonConverter.ToObject<Company>(company_str);

					Console.WriteLine("CEO.Name = " + company.CEO.Name);
					Console.WriteLine("Staffs");

					foreach (var staff in company.Staffs)
					{
						Console.WriteLine("Name = " + staff.Name);
						Console.WriteLine("Age = " + staff.Age);
					}
				}
			}
		}
	}


객체를 JSON문자열로

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	namespace SolJSONExample
	{
		class Person
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

		class Program
		{
			static void Main(string[] args)
			{
				Person person = new Person()
				{
					Name = "홍길동",
					Age = 16
				};

				Console.WriteLine("json string = " + SolJSON.Convert.JsonConverter.ToJSON(person));
			}
		}
	}


JSON Object를 사용해서 동적 JSON 파싱

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolJSONExample
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 단일 객체 동적 파싱
            {
                string person_info_str = "{\"Name\":\"홍길동\", \"Age\": 13}";
                SolJSON.Types.JsonObject person_info_json_object = SolJSON.Convert.JsonConverter.ToJsonObject(person_info_str);
                
                if ( person_info_json_object.Type == SolJSON.Types.JsonObject.TYPE.DICTONARY )
                {
                    if ( person_info_json_object.AsDictonary.Contains("Name") )
                    {
                        Console.WriteLine("Name = " + person_info_json_object.AsDictonary["Name"].AsString);
                    }
                }
            }

            // 배열 동적 파싱
            {
                string person_infos_str = "[{\"Name\":\"홍길동\", \"Age\": 13},{\"Name\":\"영희\", \"Age\": 20}]";
                SolJSON.Types.JsonObject person_infos_json_object = SolJSON.Convert.JsonConverter.ToJsonObject(person_infos_str);
                
                foreach (var person_info in person_infos_json_object.AsArray)
                {
                    Console.WriteLine("Name = " + person_info.AsDictonary["Name"].AsString);
                    Console.WriteLine("Age = " + person_info.AsDictonary["Age"].AsNumber.IntValue);
                }
            }
        }
    }
}

JSON Object를 사용해서 동적 JSON 생성

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	namespace SolJSONExample
	{

		class Program
		{
			static void Main(string[] args)
			{
				SolJSON.Types.JsonDictonary person_dict = new SolJSON.Types.JsonDictonary();
				person_dict.Add("Name", new SolJSON.Types.JsonString("홍길동"));
				person_dict.Add("Age", new SolJSON.Types.JsonNumber(13));

				Console.WriteLine("json string = " + person_dict.ToString());
			}
		}
	}

라이센스
----------

public domain
상업적으로도 자유롭게 사용가능합니다.

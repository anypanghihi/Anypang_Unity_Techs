1. Realm 데이타를 확인은 RealmStudio 를 설치해서 보는 방법이 가장 편하다. [MapTo]를 통해 매핑을 해줘야 스튜디오에서 볼 수 있음

2. Packages/manifest.json 에 아래와 같이 추가
{
 "dependencies": {
   ...
   "io.realm.unity": "10.21.0"
 },
 "scopedRegistries": [
   {
     "name": "NPM",
     "url": "https://registry.npmjs.org/",
     "scopes": [
       "io.realm.unity"
     ]
   }
 ]
}


3.https://www.mongodb.com/ko-kr/docs/atlas/device-sdks/sdk/dotnet/realm-files/realms/
4.global using 사용 방법
    1) https://github.com/Cysharp/CsprojModifier.git?path=src/CsprojModifier/Assets/CsprojModifier 설치
    2) csc.rsp 생성한뒤, 아래와 같이 적고 Assets 폴더에 추가
       -langVersion:10 -nullable
    3) LangVersion.props 생성한뒤, 아래와 같이 적고 
        <Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
          <PropertyGroup>
            <LangVersion>10</LangVersion>
            <Nullable>enable</Nullable>
          </PropertyGroup>
        </Project>
    4) Project Settings > Editor > C# Project Modifier 에 Add LangVersion.props to "Additional project imports".

    /*
    Open CsprojModifier settings:
    Project Settings > Editor > C# Project Modifier

    Add LangVersion.props to "Additional project imports".

    If you have an Assembly Definition (.asmdef):
    Add its .csproj file to "The project to be added for Import".
    */
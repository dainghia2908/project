
```
project-base-service
├─ course-app
│  ├─ .env
│  ├─ eslint.config.js
│  ├─ index.html
│  ├─ package-lock.json
│  ├─ package.json
│  ├─ postcss.config.js
│  ├─ public
│  │  └─ vite.svg
│  ├─ README.md
│  ├─ src
│  │  ├─ api
│  │  │  ├─ accountApi.ts
│  │  │  └─ axiosClient.ts
│  │  ├─ App.css
│  │  ├─ App.tsx
│  │  ├─ assets
│  │  │  └─ react.svg
│  │  ├─ components
│  │  │  └─ Dev
│  │  │     └─ RoleSwitcher.tsx
│  │  ├─ context
│  │  │  └─ MockAuthContext.tsx
│  │  ├─ features
│  │  │  └─ courses
│  │  │     ├─ api
│  │  │     │  └─ courseApi.ts
│  │  │     ├─ components
│  │  │     │  ├─ ClassResources.tsx
│  │  │     │  ├─ CreateClassForm.tsx
│  │  │     │  ├─ CreateClassModal.tsx
│  │  │     │  ├─ StaffClassManager.tsx
│  │  │     │  ├─ SubjectTable.tsx
│  │  │     │  └─ SyllabusManager.tsx
│  │  │     ├─ hooks
│  │  │     │  └─ useCourseData.ts
│  │  │     ├─ pages
│  │  │     │  ├─ ClassDetailPage.tsx
│  │  │     │  ├─ ClassListPage.tsx
│  │  │     │  ├─ ClassPage.tsx
│  │  │     │  ├─ SubjectDetailPage.tsx
│  │  │     │  └─ SubjectPage.tsx
│  │  │     └─ types
│  │  │        └─ course.types.ts
│  │  ├─ hooks
│  │  │  └─ useAuth.ts
│  │  ├─ index.css
│  │  ├─ layouts
│  │  │  └─ MainLayout.tsx
│  │  ├─ main.tsx
│  │  ├─ types
│  │  │  ├─ api.types.ts
│  │  │  └─ syllabus.types.ts
│  │  └─ utils
│  │     ├─ format.ts
│  │     └─ storage.ts
│  ├─ tailwind.config.js
│  ├─ tsconfig.app.json
│  ├─ tsconfig.json
│  ├─ tsconfig.node.json
│  └─ vite.config.ts
└─ CourseService
   ├─ appsettings.Development.json
   ├─ appsettings.json
   ├─ bin
   │  └─ Debug
   │     └─ net8.0
   │        ├─ appsettings.Development.json
   │        ├─ appsettings.json
   │        ├─ CourseService.deps.json
   │        ├─ CourseService.dll
   │        ├─ CourseService.exe
   │        ├─ CourseService.pdb
   │        ├─ CourseService.runtimeconfig.json
   │        ├─ cs
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ de
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ EPPlus.dll
   │        ├─ EPPlus.Interfaces.dll
   │        ├─ EPPlus.System.Drawing.dll
   │        ├─ es
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ fr
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ Humanizer.dll
   │        ├─ it
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ ja
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ ko
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ Microsoft.Bcl.AsyncInterfaces.dll
   │        ├─ Microsoft.CodeAnalysis.CSharp.dll
   │        ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.dll
   │        ├─ Microsoft.CodeAnalysis.dll
   │        ├─ Microsoft.CodeAnalysis.Workspaces.dll
   │        ├─ Microsoft.EntityFrameworkCore.Abstractions.dll
   │        ├─ Microsoft.EntityFrameworkCore.Design.dll
   │        ├─ Microsoft.EntityFrameworkCore.dll
   │        ├─ Microsoft.EntityFrameworkCore.Relational.dll
   │        ├─ Microsoft.Extensions.DependencyModel.dll
   │        ├─ Microsoft.IO.RecyclableMemoryStream.dll
   │        ├─ Microsoft.OpenApi.dll
   │        ├─ Microsoft.Win32.SystemEvents.dll
   │        ├─ Mono.TextTemplating.dll
   │        ├─ Npgsql.dll
   │        ├─ Npgsql.EntityFrameworkCore.PostgreSQL.dll
   │        ├─ pl
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ pt-BR
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ ru
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ runtimes
   │        │  ├─ browser
   │        │  │  └─ lib
   │        │  │     └─ net8.0
   │        │  └─ win
   │        │     └─ lib
   │        │        ├─ net7.0
   │        │        │  ├─ Microsoft.Win32.SystemEvents.dll
   │        │        │  └─ System.Drawing.Common.dll
   │        │        └─ net8.0
   │        ├─ Swashbuckle.AspNetCore.Swagger.dll
   │        ├─ Swashbuckle.AspNetCore.SwaggerGen.dll
   │        ├─ Swashbuckle.AspNetCore.SwaggerUI.dll
   │        ├─ System.CodeDom.dll
   │        ├─ System.Composition.AttributedModel.dll
   │        ├─ System.Composition.Convention.dll
   │        ├─ System.Composition.Hosting.dll
   │        ├─ System.Composition.Runtime.dll
   │        ├─ System.Composition.TypedParts.dll
   │        ├─ System.Drawing.Common.dll
   │        ├─ tr
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        ├─ zh-Hans
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │        │  ├─ Microsoft.CodeAnalysis.resources.dll
   │        │  └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   │        └─ zh-Hant
   │           ├─ Microsoft.CodeAnalysis.CSharp.resources.dll
   │           ├─ Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
   │           ├─ Microsoft.CodeAnalysis.resources.dll
   │           └─ Microsoft.CodeAnalysis.Workspaces.resources.dll
   ├─ Contronllers
   │  ├─ ClassesController.cs
   │  ├─ SubjectsController.cs
   │  └─ SyllabusesController.cs
   ├─ CourseService.csproj
   ├─ CourseService.http
   ├─ CourseService.sln
   ├─ Data
   │  └─ CourseDbContext.cs
   ├─ dotnet
   ├─ DTOs
   │  ├─ AddStudentDto.cs
   │  ├─ AssignLecturerDto.cs
   │  ├─ ClassDto.cs
   │  ├─ CreateClassDto.cs
   │  ├─ CreateSubjectDto.cs
   │  ├─ CreateSyllabusDto.cs
   │  ├─ UserDto.cs
   │  └─ UserInfoDto.cs
   ├─ Middlewares
   │  └─ GlobalExceptionMiddleware.cs
   ├─ Migrations
   │  ├─ 20260112053040_InitialCreate.cs
   │  ├─ 20260112053040_InitialCreate.Designer.cs
   │  ├─ 20260112054814_AddClassAndSyllabus.cs
   │  ├─ 20260112054814_AddClassAndSyllabus.Designer.cs
   │  ├─ 20260112060137_AddClassMembers.cs
   │  ├─ 20260112060137_AddClassMembers.Designer.cs
   │  ├─ 20260112071708_AddClassResources.cs
   │  ├─ 20260112071708_AddClassResources.Designer.cs
   │  ├─ 20260112152848_UpdateClassMemberTable.cs
   │  ├─ 20260112152848_UpdateClassMemberTable.Designer.cs
   │  ├─ 20260112162950_UpdateClassLecturerColumns.cs
   │  ├─ 20260112162950_UpdateClassLecturerColumns.Designer.cs
   │  ├─ 20260113051448_AddSyllabusAndUpdateSubject.cs
   │  ├─ 20260113051448_AddSyllabusAndUpdateSubject.Designer.cs
   │  ├─ 20260113091300_UpdateClassEntity.cs
   │  ├─ 20260113091300_UpdateClassEntity.Designer.cs
   │  ├─ 20260113152849_AddSyllabusContent.cs
   │  ├─ 20260113152849_AddSyllabusContent.Designer.cs
   │  └─ CourseDbContextModelSnapshot.cs
   ├─ Models
   │  ├─ Class.cs
   │  ├─ ClassMember.cs
   │  ├─ ClassResource.cs
   │  ├─ Subject.cs
   │  ├─ Syllabus.cs
   │  └─ SyllabusContent.cs
   ├─ obj
   │  ├─ CourseService.csproj.EntityFrameworkCore.targets
   │  ├─ CourseService.csproj.nuget.dgspec.json
   │  ├─ CourseService.csproj.nuget.g.props
   │  ├─ CourseService.csproj.nuget.g.targets
   │  ├─ Debug
   │  │  └─ net8.0
   │  │     ├─ .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
   │  │     ├─ apphost.exe
   │  │     ├─ CourseSe.9A85A0F5.Up2Date
   │  │     ├─ CourseService.AssemblyInfo.cs
   │  │     ├─ CourseService.AssemblyInfoInputs.cache
   │  │     ├─ CourseService.assets.cache
   │  │     ├─ CourseService.csproj.AssemblyReference.cache
   │  │     ├─ CourseService.csproj.CoreCompileInputs.cache
   │  │     ├─ CourseService.csproj.FileListAbsolute.txt
   │  │     ├─ CourseService.dll
   │  │     ├─ CourseService.GeneratedMSBuildEditorConfig.editorconfig
   │  │     ├─ CourseService.genruntimeconfig.cache
   │  │     ├─ CourseService.GlobalUsings.g.cs
   │  │     ├─ CourseService.MvcApplicationPartsAssemblyInfo.cache
   │  │     ├─ CourseService.MvcApplicationPartsAssemblyInfo.cs
   │  │     ├─ CourseService.pdb
   │  │     ├─ ref
   │  │     │  └─ CourseService.dll
   │  │     ├─ refint
   │  │     │  └─ CourseService.dll
   │  │     ├─ staticwebassets
   │  │     │  ├─ msbuild.build.CourseService.props
   │  │     │  ├─ msbuild.buildMultiTargeting.CourseService.props
   │  │     │  └─ msbuild.buildTransitive.CourseService.props
   │  │     └─ staticwebassets.build.json
   │  ├─ project.assets.json
   │  └─ project.nuget.cache
   ├─ Program.cs
   ├─ Properties
   │  └─ launchSettings.json
   ├─ Services
   │  ├─ ClassService.cs
   │  ├─ IClassService.cs
   │  ├─ ISubjectService.cs
   │  ├─ ISyllabusService.cs
   │  ├─ SubjectService.cs
   │  ├─ SyllabusService.cs
   │  └─ SyncDataServices
   │     ├─ AccountServiceClient.cs
   │     └─ IAccountServiceClient.cs
   ├─ Uploads
   │  ├─ 38ab07ea-d531-46a1-bc37-47315d943fb3_BAI TAP BCG.pdf
   │  ├─ a02e4481-a955-44e1-a850-8a64cbf59842_Classes.xlsx
   │  ├─ b3d62ca8-4268-49fd-af7e-dcad153ddac9_Classes.xlsx
   │  └─ fed8715e-93f3-4dd8-9a99-3cf18e2efc99_Klein, Mills, Gibson - 2006 - Traffic Detector Handbook - Volume I - Operations Research.pdf
   └─ Wrappers
      └─ ApiResponse.cs

```
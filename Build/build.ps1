properties {
  $zipFileName = "Enums.NET.2.0.0.zip"
  $majorVersion = "2.0"
  $majorWithReleaseVersion = "2.0.0"
  $nugetPrelease = $null
  $version = GetVersion $majorWithReleaseVersion
  $packageId = "Enums.NET"
  $signAssemblies = $false
  $signKeyPath = "C:\Development\Releases\enumsnet.snk"
  $buildNuGet = $true
  $treatWarningsAsErrors = $false
  $workingName = if ($workingName) {$workingName} else {"Working"}
  $netCliVersion = "1.0.0-preview3-003171"
  
  $baseDir  = resolve-path ..
  $buildDir = "$baseDir\Build"
  $sourceDir = "$baseDir\Src"
  $toolsDir = "$baseDir\Tools"
  $releaseDir = "$baseDir\Release"
  $workingDir = "$baseDir\$workingName"
  $workingSourceDir = "$workingDir\Src"
  $builds = @(
    @{Name = "Enums.NET"; TestsName = "Enums.NET.Tests"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; Constants="NET45;ENUM_MEMBER_ATTRIBUTE;SECURITY_SAFE_CRITICAL;COVARIANCE;ICONVERTIBLE;GET_TYPE_CODE;TYPE_REFLECTION"; FinalDir="Net45"; NuGetDir = "net45"; Framework="net-4.0"},
    @{Name = "Enums.NET.Net40"; TestsName = "Enums.NET.Tests.Net40"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; Constants="NET40;ENUM_MEMBER_ATTRIBUTE;SECURITY_SAFE_CRITICAL;COVARIANCE;ICONVERTIBLE;GET_TYPE_CODE;TYPE_REFLECTION"; FinalDir="Net40"; NuGetDir = "net40"; Framework="net-4.0"},
    @{Name = "Enums.NET.Net35"; TestsName = "Enums.NET.Tests.Net35"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; Constants="NET35;ENUM_MEMBER_ATTRIBUTE;ICONVERTIBLE;GET_TYPE_CODE;TYPE_REFLECTION"; FinalDir="Net35"; NuGetDir = "net35"; Framework="net-2.0"},
    @{Name = "Enums.NET.Net20"; TestsName = "Enums.NET.Tests.Net20"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; Constants="NET20;ICONVERTIBLE;GET_TYPE_CODE;TYPE_REFLECTION"; FinalDir="Net20"; NuGetDir = "net20"; Framework="net-2.0"},
    @{Name = "Enums.NET.Dotnet"; TestsName = "Enums.NET.Tests.Dotnet"; BuildFunction = "NetCliBuild"; TestsFunction = "NetCliTests"; Constants="NETSTANDARD;NETSTANDARD1_0;ENUM_MEMBER_ATTRIBUTE;SECURITY_SAFE_CRITICAL;COVARIANCE"; FinalDir="netstandard1.0"; NuGetDir = "netstandard1.0"; Framework=$null},
    @{Name = "Enums.NET.Dotnet"; TestsName = "Enums.NET.Tests.Dotnet"; BuildFunction = "NetCliBuild"; TestsFunction = "NetCliTests"; Constants="NETSTANDARD;NETSTANDARD1_3;ENUM_MEMBER_ATTRIBUTE;SECURITY_SAFE_CRITICAL;COVARIANCE;ICONVERTIBLE"; FinalDir="netstandard1.3"; NuGetDir = "netstandard1.3"; Framework=$null}
  )
}

framework '4.6x86'

task default -depends Test

# Ensure a clean working directory
task Clean {
  Write-Host "Setting location to $baseDir"
  Set-Location $baseDir
  
  if (Test-Path -path $workingDir)
  {
    Write-Host "Deleting existing working directory $workingDir"
    
    Execute-Command -command { del $workingDir -Recurse -Force }
  }
  
  Write-Host "Creating working directory $workingDir"
  New-Item -Path $workingDir -ItemType Directory
}

# Build each solution, optionally signed
task Build -depends Clean {
  Write-Host "Copying source to working source directory $workingSourceDir"
  robocopy $sourceDir $workingSourceDir /MIR /NP /XD bin obj TestResults AppPackages $packageDirs .vs artifacts /XF *.suo *.user *.lock.json | Out-Default

  Write-Host -ForegroundColor Green "Updating assembly version"
  Write-Host
  Update-AssemblyInfoFiles $workingSourceDir ($majorVersion + '.0.0') $version

  Update-Project $workingSourceDir\Enums.NET\project.json $signAssemblies

  foreach ($build in $builds)
  {
    $name = $build.Name
    if ($name -ne $null)
    {
      Write-Host -ForegroundColor Green "Building " $name
      Write-Host -ForegroundColor Green "Signed " $signAssemblies
      Write-Host -ForegroundColor Green "Key " $signKeyPath

      & $build.BuildFunction $build
    }
  }
}

# Optional build documentation, add files to final zip
task Package -depends Build {
  foreach ($build in $builds)
  {
    $name = $build.TestsName
    $finalDir = $build.FinalDir
    
    robocopy "$workingSourceDir\Enums.NET\bin\Release\$finalDir" $workingDir\Package\Bin\$finalDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
  }
  
  if ($buildNuGet)
  {
    $nugetVersion = GetNuGetVersion

    New-Item -Path $workingDir\NuGet -ItemType Directory

    $nuspecPath = "$workingDir\NuGet\Enums.NET.nuspec"
    Copy-Item -Path "$buildDir\Enums.NET.nuspec" -Destination $nuspecPath -recurse

    Write-Host "Updating nuspec file at $nuspecPath" -ForegroundColor Green
    Write-Host

    $xml = [xml](Get-Content $nuspecPath)
    Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'id']" -value $packageId
    Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'version']" -value $nugetVersion

    Write-Host $xml.OuterXml

    $xml.save($nuspecPath)

    New-Item -Path $workingDir\NuGet\tools -ItemType Directory
    
    foreach ($build in $builds)
    {
      if ($build.NuGetDir)
      {
        $name = $build.TestsName
        $finalDir = $build.FinalDir
        $frameworkDirs = $build.NuGetDir.Split(",")
        
        foreach ($frameworkDir in $frameworkDirs)
        {
          robocopy "$workingSourceDir\Enums.NET\bin\Release\$finalDir" $workingDir\NuGet\lib\$frameworkDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
        }
      }
    }
  
    robocopy $workingSourceDir $workingDir\NuGet\src *.cs /S /NFL /NDL /NJS /NC /NS /NP /XD Enums.NET.Tests Enums.NET.PerfTestConsole obj .vs artifacts | Out-Default

    Write-Host "Building NuGet package with ID $packageId and version $nugetVersion" -ForegroundColor Green
    Write-Host

    exec { .\Tools\NuGet\NuGet.exe pack $nuspecPath -Symbols }
    exec { dotnet pack $workingSourceDir\Enums.NET\project.json -c Release }
    move -Path .\*.nupkg -Destination $workingDir\NuGet
  }

  robocopy $workingSourceDir $workingDir\Package\Source\Src /MIR /NFL /NDL /NJS /NC /NS /NP /XD bin obj TestResults AppPackages .vs artifacts /XF *.suo *.user *.lock.json | Out-Default
  robocopy $buildDir $workingDir\Package\Source\Build /MIR /NFL /NDL /NJS /NC /NS /NP /XF runbuild.txt | Out-Default
  robocopy $toolsDir $workingDir\Package\Source\Tools /MIR /NFL /NDL /NJS /NC /NS /NP | Out-Default
  
  exec { .\Tools\7-zip\7za.exe a -tzip $workingDir\$zipFileName $workingDir\Package\* | Out-Default } "Error zipping"
}

# Unzip package to a location
task Deploy -depends Package {
  exec { .\Tools\7-zip\7za.exe x -y "-o$workingDir\Deployed" $workingDir\$zipFileName | Out-Default } "Error unzipping"
}

# Run tests on deployed files
task Test -depends Deploy {

  Update-Project $workingSourceDir\Enums.NET\project.json $false

  foreach ($build in $builds)
  {
    if ($build.TestsFunction -ne $null)
    {
      & $build.TestsFunction $build
    }
  }
}

function MSBuildBuild($build)
{
  $name = $build.Name
  $finalDir = $build.FinalDir

  Write-Host
  Write-Host "Restoring $workingSourceDir\$name.sln" -ForegroundColor Green
  [Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Process")
  exec { .\Tools\NuGet\NuGet.exe update -self }
  exec { .\Tools\NuGet\NuGet.exe restore "$workingSourceDir\$name.sln" -verbosity detailed -configfile $workingSourceDir\nuget.config | Out-Default } "Error restoring $name"

  $constants = GetConstants $build.Constants $signAssemblies

  Write-Host
  Write-Host "Building $workingSourceDir\$name.sln" -ForegroundColor Green
  exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:CopyNuGetImplementations=true" "/p:Platform=Any CPU" "/p:PlatformTarget=AnyCPU" /p:OutputPath=bin\Release\$finalDir\ /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$signAssemblies" "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" /p:DefineConstants=`"$constants`" "$workingSourceDir\$name.sln" | Out-Default } "Error building $name"
}

function NetCliBuild($build)
{
  $name = $build.Name
  $framework = $build.NuGetDir
  $projectPath = "$workingSourceDir\Enums.NET\project.json"

  exec { .\Tools\Dotnet\dotnet-install.ps1 -Version $netCliVersion | Out-Default }
  exec { dotnet --version | Out-Default }

  Write-Host -ForegroundColor Green "Restoring packages for $name"
  Write-Host
  exec { dotnet restore $projectPath | Out-Default }

  Write-Host -ForegroundColor Green "Building $projectPath $framework"
  exec { dotnet build $projectPath -f $framework -c Release -o bin\Release\$framework | Out-Default }
}

function NetCliTests($build)
{
  $name = $build.TestsName

  exec { .\Tools\Dotnet\dotnet-install.ps1 -Version $netCliVersion | Out-Default }
  exec { dotnet --version | Out-Default }

  Write-Host -ForegroundColor Green "Restoring packages for $name"
  Write-Host
  exec { dotnet restore "$workingSourceDir\Enums.NET.Tests\project.json" | Out-Default }

  Write-Host -ForegroundColor Green "Ensuring test project builds for $name"
  Write-Host

  try
  {
    Set-Location "$workingSourceDir\Enums.NET.Tests"
    exec { dotnet test "$workingSourceDir\Enums.NET.Tests\project.json" -f netcoreapp1.0 -c Release | Out-Default }
  }
  finally
  {
    Set-Location $baseDir
  }
}

function NUnitTests($build)
{
  $name = $build.TestsName
  $finalDir = $build.FinalDir
  $framework = $build.Framework

  Write-Host -ForegroundColor Green "Copying test assembly $name to deployed directory"
  Write-Host
  robocopy "$workingSourceDir\Enums.NET.Tests\bin\Release\$finalDir" $workingDir\Deployed\Bin\$finalDir /MIR /NFL /NDL /NJS /NC /NS /NP /XO | Out-Default

  Copy-Item -Path "$workingSourceDir\Enums.NET.Tests\bin\Release\$finalDir\Enums.NET.Tests.dll" -Destination $workingDir\Deployed\Bin\$finalDir\

  Write-Host -ForegroundColor Green "Running NUnit tests " $name
  Write-Host
  exec { .\Tools\NUnit\nunit3-console.exe "$workingDir\Deployed\Bin\$finalDir\Enums.NET.Tests.dll" /framework=$framework | Out-Default } "Error running $name tests"
}

function GetNuGetVersion()
{
  $nugetVersion = $majorWithReleaseVersion
  if ($nugetPrerelease -ne $null)
  {
    $nugetVersion = $nugetVersion + "-" + $nugetPrerelease
  }

  return $nugetVersion
}

function GetConstants($constants, $includeSigned)
{
  $signed = if ($includeSigned) {";SIGNED"} else {""}

  return "CODE_ANALYSIS;TRACE;$constants$signed"
}

function GetVersion($majorVersion)
{
    $now = [DateTime]::Now
    
    $year = $now.Year - 2000
    $month = $now.Month
    $totalMonthsSince2000 = ($year * 12) + $month
    $day = $now.Day
    $revision = "{0}{1:00}" -f $totalMonthsSince2000, $day
    
    return $majorVersion + "." + $revision
}

function Update-AssemblyInfoFiles ([string] $workingSourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber)
{
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
    $fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
    
    Get-ChildItem -Path $workingSourceDir -r -filter AssemblyInfo.cs | ForEach-Object {
        
        $filename = $_.Directory.ToString() + '\' + $_.Name
        Write-Host $filename
        $filename + ' -> ' + $version
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}

function Edit-XmlNodes {
    param (
        [xml] $doc,
        [string] $xpath = $(throw "xpath is a required parameter"),
        [string] $value = $(throw "value is a required parameter")
    )
    
    $nodes = $doc.SelectNodes($xpath)
    $count = $nodes.Count

    Write-Host "Found $count nodes with path '$xpath'"
    
    foreach ($node in $nodes) {
        if ($node -ne $null) {
            if ($node.NodeType -eq "Element")
            {
                $node.InnerXml = $value
            }
            else
            {
                $node.Value = $value
            }
        }
    }
}

function Update-Project {
  param (
    [string] $projectPath,
    [Boolean] $sign
  )

  $file = if ($sign) {$signKeyPath} else {$null}

  $json = (Get-Content $projectPath) -join "`n" | ConvertFrom-Json
  $options = @{"warningsAsErrors" = $true; "xmlDoc" = $true; "keyFile" = $file; "define" = ((GetConstants "dotnet" $sign) -split ";") }
  Add-Member -InputObject $json -MemberType NoteProperty -Name "buildOptions" -Value $options -Force

  $json.version = GetNuGetVersion

  ConvertTo-Json $json -Depth 10 | Set-Content $projectPath
}

function Execute-Command($command) {
    $currentRetry = 0
    $success = $false
    do {
        try
        {
            & $command
            $success = $true
        }
        catch [System.Exception]
        {
            if ($currentRetry -gt 5) {
                throw $_.Exception.ToString()
            } else {
                write-host "Retry $currentRetry"
                Start-Sleep -s 1
            }
            $currentRetry = $currentRetry + 1
        }
    } while (!$success)
}
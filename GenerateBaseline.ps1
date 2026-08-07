### .\GenerateBaseline.ps1 > EGPv2_Baseline.txt
$exclude = @(
    "bin",
    "obj",
    ".git",
    ".vs",
    "node_modules",
    "QUERY_TEST_01.sql",
    "QUERY_TEST_02.sql",
    "QUERY_TEST_03.sql",
    "EGPv2.zip",
    "00.Start.md",
    "GenerateBaseline.ps1",
    "EGPv2_Baseline.txt"
)

function ShowTree($path, $indent)
{
    Get-ChildItem $path |
    Where-Object { $exclude -notcontains $_.Name } |
    Sort-Object PSIsContainer -Descending |
    ForEach-Object {

        Write-Output "$indent$($_.Name)"

        if ($_.PSIsContainer)
        {
            ShowTree $_.FullName "$indent    "
        }
    }
}

ShowTree "." ""
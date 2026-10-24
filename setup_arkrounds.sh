#!/usr/bin/env bash
# ArkRounds - setup + build for Arch Linux (Steam + Proton)
#
# Put this script in the same folder as ArkRounds.cs, then run:
#   bash setup_arkrounds.sh
#
# The final DLL ends up in:  ~/Documents/ArkRounds/ArkRounds.dll
#
# Optional overrides (set before the command):
#   GAME_DIR=/other/path/ROUNDS   OUT_DIR=/other/output/folder
#   SKIP_MODS=1    never touch BepInEx/UnboundLib (build only)
#   FORCE_MODS=1   re-download BepInEx/UnboundLib even if already installed
#   INSTALL=1      also copy the built DLL into the game's BepInEx/plugins folder

set -euo pipefail

GAME_DIR="${GAME_DIR:-$HOME/.local/share/Steam/steamapps/common/ROUNDS}"
OUT_DIR="${OUT_DIR:-$HOME/Documents/ArkRounds}"
SRC_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TMP="$(mktemp -d)"
trap 'rm -rf "$TMP"' EXIT

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# ---------------------------------------------------------------- checks
if [[ ! -d "$GAME_DIR" ]]; then
  echo "ERROR: ROUNDS folder not found: $GAME_DIR"
  exit 1
fi

if [[ ! -f "$SRC_DIR/ArkRounds.cs" ]]; then
  echo "ERROR: ArkRounds.cs is missing from $SRC_DIR (put this script next to it)"
  exit 1
fi

# The Unity data folder is "<ExeName>_Data" - match it case-insensitively
# (ROUNDS_Data vs Rounds_Data) since Linux paths are case-sensitive.
MANAGED_DIR="$(find "$GAME_DIR" -maxdepth 2 -type d -ipath '*_data/managed' -print -quit 2>/dev/null || true)"
if [[ -z "$MANAGED_DIR" || ! -f "$MANAGED_DIR/Assembly-CSharp.dll" ]]; then
  echo "ERROR: could not find Assembly-CSharp.dll under $GAME_DIR"
  echo "Contents of the game folder:"
  ls -la "$GAME_DIR"
  exit 1
fi
echo "==> Game assemblies: $MANAGED_DIR"

# ---------------------------------------------------------------- build tools
pkgs=()
command -v dotnet >/dev/null || pkgs+=(dotnet-sdk)
command -v curl   >/dev/null || pkgs+=(curl)
command -v unzip  >/dev/null || pkgs+=(unzip)
command -v jq     >/dev/null || pkgs+=(jq)
if (( ${#pkgs[@]} )); then
  echo "==> Installing build tools (needs sudo): ${pkgs[*]}"
  sudo pacman -S --needed --noconfirm "${pkgs[@]}"
fi

# ---------------------------------------------------------------- mods
declare -A SEEN=()

# Downloads the latest version of a Thunderstore package (and its dependencies).
# Packages containing winhttp.dll (the BepInEx pack) are unpacked into the game folder;
# everything else goes to BepInEx/plugins/<Owner>-<Name>/.
install_pkg() {
  local owner="$1" name="$2" json dep d_owner rest d_name version zip dir proxy

  if [[ -n "${SEEN[$owner/$name]:-}" ]]; then return 0; fi
  SEEN[$owner/$name]=1

  echo "==> Thunderstore: $owner/$name"
  json="$(curl -fsSL "https://thunderstore.io/api/experimental/package/$owner/$name/")"

  # dependencies look like "Owner-Name-1.2.3"
  while read -r dep; do
    [[ -z "$dep" ]] && continue
    d_owner="${dep%%-*}"
    rest="${dep#*-}"
    d_name="${rest%-*}"
    install_pkg "$d_owner" "$d_name"
  done < <(jq -r '.latest.dependencies[]' <<<"$json")

  version="$(jq -r '.latest.version_number' <<<"$json")"
  zip="$TMP/$owner-$name-$version.zip"
  dir="$TMP/x-$owner-$name"
  rm -rf "$dir"; mkdir -p "$dir"

  curl -fsSL -o "$zip" "https://thunderstore.io/package/download/$owner/$name/$version/"
  unzip -q -o "$zip" -d "$dir"

  proxy="$(find "$dir" -iname winhttp.dll -print -quit)"
  if [[ -n "$proxy" ]]; then
    echo "    -> BepInEx pack, installing into game folder"
    cp -a "$(dirname "$proxy")/." "$GAME_DIR/"
  else
    echo "    -> plugin, installing into BepInEx/plugins/$owner-$name"
    mkdir -p "$GAME_DIR/BepInEx/plugins/$owner-$name"
    cp -a "$dir/." "$GAME_DIR/BepInEx/plugins/$owner-$name/"
  fi
}

mods_present() {
  [[ -f "$GAME_DIR/BepInEx/core/BepInEx.dll" ]] &&
  [[ -n "$(find "$GAME_DIR/BepInEx" -name UnboundLib.dll -print -quit 2>/dev/null)" ]]
}

if [[ "${SKIP_MODS:-0}" != "1" ]]; then
  if [[ "${FORCE_MODS:-0}" == "1" ]] || ! mods_present; then
    install_pkg willis81808 UnboundLib
  else
    echo "==> BepInEx + UnboundLib already installed, skipping download"
  fi
fi

CORE_DIR="$GAME_DIR/BepInEx/core"
UNBOUND_DLL="$(find "$GAME_DIR/BepInEx" -name UnboundLib.dll -print -quit 2>/dev/null || true)"

if [[ ! -f "$CORE_DIR/BepInEx.dll" || ! -f "$CORE_DIR/0Harmony.dll" ]]; then
  echo "ERROR: BepInEx core DLLs not found in $CORE_DIR"
  exit 1
fi
if [[ -z "$UNBOUND_DLL" ]]; then
  echo "ERROR: UnboundLib.dll not found under $GAME_DIR/BepInEx"
  exit 1
fi
echo "==> UnboundLib: $UNBOUND_DLL"

# ---------------------------------------------------------------- generate project
BUILD_DIR="$TMP/build"
mkdir -p "$BUILD_DIR"
cp "$SRC_DIR/ArkRounds.cs" "$BUILD_DIR/"

# Quoted heredoc: nothing is expanded by the shell; paths come in via -p: properties.
cat > "$BUILD_DIR/ArkRounds.csproj" <<'EOF'
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net472</TargetFramework>
    <AssemblyName>ArkRounds</AssemblyName>
    <RootNamespace>ArkRounds</RootNamespace>
    <LangVersion>latest</LangVersion>
    <DebugType>none</DebugType>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
  </PropertyGroup>

  <ItemGroup>
    <!-- lets net472 compile on Linux without Mono / .NET Framework -->
    <PackageReference Include="Microsoft.NETFramework.ReferenceAssemblies" Version="1.0.3" PrivateAssets="all" />
  </ItemGroup>

  <ItemGroup>
    <!-- Game + Unity + Photon (wildcards so any type the game's classes depend on resolves) -->
    <Reference Include="$(ManagedDir)/Assembly-CSharp*.dll"><Private>false</Private></Reference>
    <Reference Include="$(ManagedDir)/UnityEngine*.dll"><Private>false</Private></Reference>
    <Reference Include="$(ManagedDir)/Unity.*.dll"><Private>false</Private></Reference>
    <Reference Include="$(ManagedDir)/Photon*.dll"><Private>false</Private></Reference>

    <!-- BepInEx + UnboundLib -->
    <Reference Include="$(CoreDir)/BepInEx.dll"><Private>false</Private></Reference>
    <Reference Include="$(CoreDir)/0Harmony.dll"><Private>false</Private></Reference>
    <Reference Include="$(UnboundDll)"><Private>false</Private></Reference>
  </ItemGroup>

</Project>
EOF

# ---------------------------------------------------------------- build
echo "==> Building"
dotnet build "$BUILD_DIR/ArkRounds.csproj" -c Release -o "$BUILD_DIR/out" \
  -p:ManagedDir="$MANAGED_DIR" \
  -p:CoreDir="$CORE_DIR" \
  -p:UnboundDll="$UNBOUND_DLL"

if [[ ! -f "$BUILD_DIR/out/ArkRounds.dll" ]]; then
  echo "ERROR: build finished but ArkRounds.dll was not produced"
  exit 1
fi

mkdir -p "$OUT_DIR"
cp -f "$BUILD_DIR/out/ArkRounds.dll" "$OUT_DIR/ArkRounds.dll"

if [[ "${INSTALL:-0}" == "1" ]]; then
  mkdir -p "$GAME_DIR/BepInEx/plugins/ArkRounds"
  cp -f "$OUT_DIR/ArkRounds.dll" "$GAME_DIR/BepInEx/plugins/ArkRounds/ArkRounds.dll"
  echo "==> Also installed into $GAME_DIR/BepInEx/plugins/ArkRounds/"
fi

# ---------------------------------------------------------------- done
echo
echo "==> Done. Your DLL:"
echo "    $OUT_DIR/ArkRounds.dll"
echo
if [[ "${INSTALL:-0}" != "1" ]]; then
  echo "To install it into the game:"
  echo "    mkdir -p \"$GAME_DIR/BepInEx/plugins/ArkRounds\" && cp \"$OUT_DIR/ArkRounds.dll\" \"$GAME_DIR/BepInEx/plugins/ArkRounds/\""
  echo
fi
echo "Steam launch option (needed so Proton loads BepInEx):"
echo "    WINEDLLOVERRIDES=\"winhttp=n,b\" %command%"
echo
echo "Check the log after launching the game:"
echo "    tail -f \"$GAME_DIR/BepInEx/LogOutput.log\""

<h1 align="center"> 📜Haitong Extraction System </h1>

<p align="center">
  Automate extract generation and create custom menus based on JSONs and instructions for precise keyboard input control.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/language-C%23-blue.svg?color=rgb(170%2C%2069%2C%20145)" />
  <img src="https://img.shields.io/badge/platform-Windows-lightgrey.svg?color=blue" />
  <img src="https://img.shields.io/badge/.NET-blue?color=green" />
  <img src="https://img.shields.io/github/issues/zalaszz/HES" />
  <img alt="GitHub Downloads (all assets, all releases)" src="https://img.shields.io/github/downloads/zalaszz/HES/total?color=green">
  <img src="https://img.shields.io/github/v/release/zalaszz/HES" />
</p>


# Features

- [Features](#features)
  - [🚀 Overview](#-overview)
  - [💻 Requirements](#-requirements)
  - [⚡ Installation](#-installation)
    - [1. Clone the repository](#1-clone-the-repository)
    - [2. Navigate to the project folder](#2-navigate-to-the-project-folder)
    - [3. Restore dependencies](#3-restore-dependencies)
    - [4. Build the project](#4-build-the-project)
    - [5. Run the application](#5-run-the-application)
  - [🌐 Support for Other Platforms](#-support-for-other-platforms)
  - [🔧 How To Use](#-how-to-use)
    - [Virtual Keys Mapping and Configuration](#virtual-keys-mapping-and-configuration)
    - [Configuration in Settings File](#configuration-in-settings-file)
    - [JSON Configuration for Menu Fields](#json-configuration-for-menu-fields)
    - [Instructions and Loops](#instructions-and-loops)
    - [CSV File Usage](#csv-file-usage)

## 🚀 Overview

This application simulates keystrokes using the SendInput function and the Win32 API. By leveraging these tools, it automates keyboard input to perform actions quickly and efficiently, mimicking real user interactions. The use of SendInput allows for precise control over keystrokes, while the Win32 API ensures seamless integration with the Windows operating system, enabling smooth and reliable automation of repetitive tasks.

---

## 💻 Requirements

Before you begin, make sure you have the following installed:

- [Visual Studio](https://visualstudio.microsoft.com/) or another compatible C# code editor 💻
- [.NET SDK](https://dotnet.microsoft.com/download) (.NET 4.6.1) 🔧
- Operating System: Windows 🖥️ (for other platforms, see the [Support for Other Platforms](#support-for-other-platforms) section).

---

## ⚡ Installation

To get the project set up locally, follow these steps:

### 1. Clone the repository

Open your terminal or command prompt and run the following command to clone the repository:

```bash
git clone https://github.com/zalaszz/HES.git
```

### 2. Navigate to the project folder

```bash
cd HES/
```

### 3. Restore dependencies

Run the following command to restore the project's dependencies:

```bash
dotnet restore
```

### 4. Build the project

Build the project using this command:

```bash
dotnet build
```

### 5. Run the application

Now, you can run the application:

```bash
dotnet run
```

---

## 🌐 Support for Other Platforms

The project was primarily developed for Windows, so it won't run on other platforms such as Linux and macOS.

Support will **NOT** be provided for other platforms.

---

## 🔧 How To Use
### Virtual Keys Mapping and Configuration

The following virtual keys are available and should be prefixed with the `virtualkey:` before the key name in the `instructions.json` file located in the `Resources/` directory:

<details>
  <summary>Virtual Keys List</summary>
  
  - **ENTER**: `virtualkey:ENTER`
  - **AT_SIGN**: `virtualkey:AT_SIGN`
  - **CAPSLOCK**: `virtualkey:CAPSLOCK`
  - **BACKSPACE**: `virtualkey:BACKSPACE`
  - **CTRL**: `virtualkey:CTRL`
  - **ALT**: `virtualkey:ALT`
  - **LALT**: `virtualkey:LALT`
  - **RALT**: `virtualkey:RALT`
  - **SPACEBAR**: `virtualkey:SPACEBAR`
  - **LSHIFT**: `virtualkey:LSHIFT`
  - **PERIOD**: `virtualkey:PERIOD`
  - **COMMA**: `virtualkey:COMMA`
  - **DIVIDE**: `virtualkey:DIVIDE`
  - **MULTIPLY**: `virtualkey:MULTIPLY`
  - **PLUS**: `virtualkey:PLUS`
  - **SEPARATOR**: `virtualkey:SEPARATOR`
  - **MINUS**: `virtualkey:MINUS`
  - **END**: `virtualkey:END`
  - **UP_ARROW**: `virtualkey:UP_ARROW`
  - **DOWN_ARROW**: `virtualkey:DOWN_ARROW`
  - **VK_0**: `virtualkey:VK_0`
  - **VK_1**: `virtualkey:VK_1`
  - **VK_2**: `virtualkey:VK_2`
  - **VK_3**: `virtualkey:VK_3`
  - **VK_4**: `virtualkey:VK_4`
  - **VK_5**: `virtualkey:VK_5`
  - **VK_6**: `virtualkey:VK_6`
  - **VK_7**: `virtualkey:VK_7`
  - **VK_8**: `virtualkey:VK_8`
  - **VK_9**: `virtualkey:VK_9`
  - **VK_A**: `virtualkey:VK_A`
  - **VK_B**: `virtualkey:VK_B`
  - **VK_C**: `virtualkey:VK_C`
  - **VK_D**: `virtualkey:VK_D`
  - **VK_E**: `virtualkey:VK_E`
  - **VK_F**: `virtualkey:VK_F`
  - **VK_G**: `virtualkey:VK_G`
  - **VK_H**: `virtualkey:VK_H`
  - **VK_I**: `virtualkey:VK_I`
  - **VK_J**: `virtualkey:VK_J`
  - **VK_K**: `virtualkey:VK_K`
  - **VK_L**: `virtualkey:VK_L`
  - **VK_M**: `virtualkey:VK_M`
  - **VK_N**: `virtualkey:VK_N`
  - **VK_O**: `virtualkey:VK_O`
  - **VK_P**: `virtualkey:VK_P`
  - **VK_Q**: `virtualkey:VK_Q`
  - **VK_R**: `virtualkey:VK_R`
  - **VK_S**: `virtualkey:VK_S`
  - **VK_T**: `virtualkey:VK_T`
  - **VK_U**: `virtualkey:VK_U`
  - **VK_V**: `virtualkey:VK_V`
  - **VK_W**: `virtualkey:VK_W`
  - **VK_X**: `virtualkey:VK_X`
  - **VK_Y**: `virtualkey:VK_Y`
  - **VK_Z**: `virtualkey:VK_Z`
  - **F6**: `virtualkey:F6`
  - **F11**: `virtualkey:F11`
</details>

These key names should be used when writing instructions for automation in the `instructions.json` file.

---

### Configuration in Settings File

In the `settings.json` file (located in the `Resources/` directory), you can define the following dictionaries:

- **SpecialChars**: Mapping of virtual keys to special characters (symbols).
- **SpecialShiftChars**: Mapping for special characters with the SHIFT key pressed.
- **SpecialAltChars**: Mapping for special characters with the ALT key pressed.

These mappings depend on the keyboard layout. The default layout is **pt-PT**.

---

### JSON Configuration for Menu Fields

The `menu.json` file (located in the `Resources/` directory) allows you to specify the fields for user input, with the following 6 types:

|Type	 |Description |
|------|------------|
|Text	| A regular text input field|
|MultiText |	A multi-text input field (accepts multiple words separated by space)|
|Number	| A numerical input field (only numbers)|
|MultiNumber | A multi-number input field (accepts multiple numbers separated by space)|
|Date |	A date input field (typically for selecting a date)|
|MultiDate |	A multi-date input field (accepts multiple dates separated by space)|
|Hidden |	A hidden input field (usually for passwords)|

The **multi** type fields were created to be used in the `instructions.json` file under the `iterations` section, formatted as `field:<field_name>`.

Example configuration:

```json
{
  "LoginFields": [
    {
      "name": "username",
      "type": "text"
    },
    {
      "name": "password",
      "type": "hidden"
    }
  ],
  "AdditionalFields": [
    {
      "name": "cifs",
      "type": "multiNumber"
    },
    {
      "name": "start date",
      "type": "date"
    },
    {
      "name": "end date",
      "type": "date"
    }
  ]
}
```

In this example, the `LoginFields` are typically used for login information (like username and password), and `AdditionalFields` allow extra data like CIFs and dates.

---

### Instructions and Loops

Instructions in the `instructions.json` file can be used to specify the actions to automate.

You can also create loops by specifying the number of `iterations`. If the iterations property is not set, the default is `1`. If a field is of type multi (such as `multiText` or `multiNumber`) and contains values separated by spaces, the number of values separated by those spaces will be used to set the number of iterations. Alternatively, you can set iterations to `infinite` or `-1` for an indefinite loop, which can be stopped using the `F6 key`.

In the instructions, you can use:

- `field:<field_name>`, the value for that field (e.g., `field:username`) will be used instead.
- `virtualkey:<key_name>` for [virtual keys](#virtual-keys-mapping-and-configuration) (e.g., `virtualkey:ENTER`, `virtualkey:SPACEBAR`, etc.).
- Text without any prefix for simple characters or strings (e.g., `"Hello"` or `"1234"`).


---

### CSV File Usage
You can place a CSV file with numeric data in the `in/` folder inside the program directory. The file should not contain any letters and must only have numeric values.

For example, the CSV file could have the following columns:

| Cif    | Start Date | End Date   |
|--------|------------|------------|
| 1234   | 2023.01.01 | 2023.12.31 |
| 5678   | 2023.02.01 | 2023.11.30 |


Once the file is read, it will be placed in the `out/` folder inside the program directory.

***note:** Don't forget to remove the headers Cif, Start Date and End Date*

---

<p align="center">Made by zalaszz ☣️</p>

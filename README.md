# Doclava

Doclava is a doclet for javadoc, originally developed by Google. The original code is located at https://android.googlesource.com/platform/external/doclava/
The advantages of this doclet over the standard doclet are:
- Easiness to customise the output, because a templating engine is used (clearsilver).
- Direct URLs to each page, because it generates static pages instead of frames.

This project contains customised source code for Doclava, so it generates output with a look and feel similar to Ooyala's other documentation.

## Build the doclet

Note: for Unix based operating systems only.

### Prerequisite

- JDK 1.8 or higher
- Maven

You can install Maven through following instructions on its [website](https://maven.apache.org/index.html), or through Homebrew:
```
brew install maven
```

### Build command

Execute the following command from the repo's root folder:
```
mvn clean package
```

## How to use the doclet locally?

Run javadoc, using the `-doclet` and `-docletpath` command options. For example:

```
javadoc @argsfile -doclet com.google.doclava.Doclava -docletpath path/to/file/doclava-<version>.jar
```

The `argsfile` may contain all standard javadoc command options (excluding the standard doclet options, see [javadoc](https://docs.oracle.com/javase/8/docs/technotes/tools/windows/javadoc.html)), and the options for the Doclava doclet.

### Standard javadoc command option exceptions

Keep the following limitations in mind when using the standard javadoc command options:

- `-Xdoclint`: although listed in the javadoc documentation as a javadoc option, it is actually one from the standard doclet, which means that it is not recognised when using in combination with any another doclet.

### Doclava doclet command options

- `-d`: Specify the directory to put the output files.

- `-templatedir`: Specify the directory of clearsilver template files. You can have as many of these as you need.

- `-hdf`: Provide name and value pairs to set in the Clearsilver hdf namespace. Again, you can have as many of these as you need. These are properties used in Doclava's templates. The following properties are **mandatory** to set due to Ooyala's customisations:
  - `project.name`: Set the name for the SDK reference project. This property is used in the title tag of each page. For example: `-hdf project.name 'Ooyala Android SDK for Video Platform'`.
  - `project.product_line`: Set the name of the product line to which the SDK belongs. For example: `-hdf project.product_line 'Video Platform'`.
  - `project.sdk_name`: Set the name of the SDK. For example: `-hdf project.sdk_name 'Android SDK'`


- `-htmldir`: Specify the source directory for html/jd files.

  You can also put other files, like images, in this directory and they are copied to the output directory.

  See "Narrative Documentation" below for more information on what goes in this directory.

- `-title`: Specify a string to insert in the html page title. The full title of each page is `<page name> - <title> | <project.name>`, where:
  - `<page name>` is the package name in case of pages describing packages, or the class name in case of pages describing classes.
  - `<title>` is the string you have specified in the `-title` option.
  - `<project.name>` is the string you have specified in the `-hdf project.name` option.

  It's recommended not to use this option.


- `-werror`: Set this if you want anything that would be a warning to be an error. You can use -error, -warning or -hide for finer-grained control. See below.

- `-error` number

  `-warning` number

  `-hide` number

  Specify whether a given error number is an error, warning, or whether to hide it altogether. These are common documentation mistakes that can easily be caught by the tool and increase the general quality and reduce the amount of proofreading necessary. None of these default to error because the Sun doclet allows most of these errors, and defaulting to errors would make most Java code not produce docs by default. The best practice is to turn the warnings into errors one at a time once you have eliminated them.

| Error Number | Default | Description |
|-----|------------|----------------|
| 101 | warning | Unresolved @link or @see tag. |
| 102 | warning | Bad {@sample } or {@include } tag. |
| 103 | warning | Unknown tag. Check for misspellings like @returns instead of @return, @throw instead of @throws, etc. |
| 104 | warning | Bad @param tag name. An @param tag references a parameter name that doesn't exist in the given method. |
| 105 | hidden | Undocumented Parameter. There is a parameter that doesn't have a corresponding @param tag. This is really really common, so it's hidden by default |
| 113 | warning | Mismatch between @Deprecated annotation (present) and @deprecated tag containing the information on deprecation (not present) |


- `-proofread`: Specify a proofread file (.txt). A proofread file contains all of the text content of the javadocs concatenated into one file, suitable for spell-checking and other goodness. The path to the proofread file is relative to where you run the javadoc command.

- `-todo`: Specify a todo file (.html). A todo file lists the program elements that are missing documentation. At some point, this might be improved to show more warnings. The path to the todo file is relative to the output directory specified with the `-d` option.

- `-public`: Only show documentation from public classes and members.

  `-protected`: Only show documentation from protected and public classes and members. This is the default.

  `-package`: Only show documentation from package, protected and public classes and members.

  `-private`: Show documentation for all classes and members, except hidden classes and members.

  `-hidden`: Show documentation for all classes and members, even when tagged with @hide.


- `-since` version.txt name

  Provide information about when API features were added to the project. The version files are created with Doclava, running it with the option `-nodocs` and `-api`. You can add as many `-since` options to the arguments file as needed, but they need to be added in the correct order of the versions, starting from the lowest version. For example:

  ```
  -since 1.txt 1
  -since 2.txt 2
  -since 3.txt 3
  -since 4.txt 4
  ...
  ```

  However, Doclava currently only supports integers as version names to obtain the correct behaviour in the output. If you enter anything else as the version name, you need to be certain that the String comparison used in Javascript (see [The Abstract Relational Comparison Algorithm](http://www.ecma-international.org/ecma-262/5.1/#sec-11.8.5) in ECMA-5) can put the versions in the correct order. An example of version names that do not work:

  ```
  -since 2.3.17.8.0.txt 2.3.17.8.0
  -since 2.3.17.9.0.txt 2.3.17.9.0
  -since 2.3.17.10.0.txt 2.3.17.10.0
  -since 2.3.17.12.0.txt 2.3.17.12.0
  -since 2.4.17.14.0.txt 2.4.17.14.0
  ```

  The String comparison compares the versions character by character. So, when comparing version `2.3.17.8.0` with version `2.3.17.10.0`, version `2.3.17.10.0` is considered less than version `2.3.17.8.0`, because the first difference in the Strings are `8` and `1`.

  Due to this limitation, it is recommended not to use `-since`.

- `-federate` name site

  Link against an external documentation site. For more information, see FederatedDocs

- `-federationapi` name file

  You can specify an alternative txt file when federating against an external site. This can be useful for federating against sites that use the same layout for reference documentation, but do not provide a current.txt file.

- `-nodocs`

  If Doclava is being run for a purpose other than generating documents, like generating the version.txt file, then use this flag to achieve this goal and speed up the build process.

- `-parsecomments`

  If Doclava is being run for a purpose other than generating documentation, comments will not be parsed unless this flag is set. Not parsing comments can speed up builds for large projects, but will result in errors in those comments not being reported.

- `-toroot` path

  Prepends the given path to any generated relative URL or fragment. Useful for specifying an absolute path.

- `-yaml` filename.ext

  Generates a YAML file named filename.ext that contains a list of the package, class, interface and exception names with links to their files. You can convert this to a navigation menu or table of contents for the reference documentation. The output YAML file is created in the assets folder. You normally set .ext to the .yaml extension.

- `-knowntags` file

  This is the location of a file listing the potential extra tags that have been used in the project. See https://android.googlesource.com/platform/frameworks/base/+/refs/heads/master/docs/knowntags.txt for an example.

- `-samplecode` <todo>

- `-samplecodegroup` <todo>

- `-samplesdir` <todo>

- `htmldir2` <todo>

- `keeplist` <todo>

- `showAnnotation` <todo>

- `-showAnnotationShowsVisibility` <todo>

- `-hidePackage` <todo>

- `-proguard` <todo>

- `-stubs` <todo>

- `-stubpackages` <todo>

- `-sdkvalues` <todo>

- `-api` <todo>

- `-removedApi` <todo>

- `-nodefaultassets` <todo>

- `-offlinemode` <todo>

- `-metadataDebug` <todo>

- `-includePreview` <todo>

- `-ignoreJdLinks` <todo>

- `-documentannotations` <todo>

- `-referenceonly` <todo>

- `-staticonly` <todo>

- `-navtreeonly` <todo>

- `-atLinksNavtree` <todo>

- `-devsite` <todo>

## Narrative Documentation

If -htmldir is given on the commandline or in the argsfile, then Doclava reads all files in the given directory, including subdirectories, and depending on the extension of the file, takes one of the following actions:

### .jd files

A .jd file is a javadoc file, and consists of a set of Clearsilver hdf definitions, followed by "@jd:body", and then the body text of the document. For example, take a file called `filename.jd`:

```html
<!-- Start with any Clearsilver hdf definitions -->
page.title=Android SDK

<!-- Provide the body content of the page -->
@jd:body
<h1>Android SDK</h1>
<p>Welcome to Android!</p>
<p>This is a link to a class called {@link android.view.View}.</p>
```

The standard Doclava templates use the hdf node page.title to set the html <title> tag.

`filename.jd` undergoes the following operations:
1. It is run through the javadoc processor. All inline javadoc tags work (like {@link}). No out-of-line javadoc tags work (what would @return do anyway in this context?).
2. It is wrapped with the standard formatting of the documentation pages, that comes from the Clearsilver templates.
3. The output is then saved to `filename.html` in the output directory, respecting the originating directory's hierarchy.

### .cs files

The file is processed as a Clearsilver template file and saved to filename.html in the output directory, respecting the originating directory's hierarchy. Javadoc processing is not performed as it would be with .jd files.

### All other files

The file is copied to the output directory unchanged, respecting the originating directory's hierarchy.


## Javadoc tags in Doclava

Doclava adds support for a number of Javadoc tags, and does not handle others that the standard doclet does.

### New or Improved Tags

@hide

When applied to a package, class, method or field, @hide removes that node and all of its children from the documentation.

@deprecated

Puts a warning message that you shouldn't use this particular class, method or field. The warning is a little different from Sun's though: * In the summary listing, they also list the brief description of the element. We only put the warning in the summary list, and make the user scroll down to the full description to get any info about what it does. This is to further discourage use of deprecated program elements. * When a class inherits from a deprecated class or overrides a deprecated method, the deprecated warning automatically propagates into that documentation. (This is not done when a class inherits from a deprecated interface). You can add @undeprecate to the comments for the subclass / overridden method if you want to suppress this behavior.

@undeprecate

Don't inherit @deprecated from your superclass or overridden method. See @deprecated above.

{@more}

The Sun javadoc always ends the brief description of a program element at the first period. If you have more to say in the brief description, or have markup that has periods, put {@more} where you want the break to be. Everything after that will be in the full description.

{@sample} and {@include}

These tags copy sample text from an arbitrary file into the output javadoc html.

The @include tag copies the text verbatim from the given file.

The @sample tag copies the text from the given file and * strips leading and trailing whitespace * reduces the indent level of the text to the indent level of the first non-whitespace line * escapes all <, >; and & characters for html * drops all lines containing either BEGIN_INCLUDE or END_INCLUDE so sample code can be nested

Both tags accept either a filename and an id or just a filename. If no id is provided, the entire file is copied. If an id is provided, the lines in the given file between the first two lines containing BEGIN_INCLUDE(id) and END_INCLUDE(id), for the given id, are copied. The id may be only letters, numbers and underscore ().

Four examples: {@include samples/SampleCode/src/com/google/app/Notification1.java} {@sample samples/SampleCode/src/com/google/app/Notification1.java} {@include samples/SampleCode/src/com/google/app/Notification1.java Bleh} {@sample samples/SampleCode/src/com/google/app/Notification1.java Bleh}

@attr

The @attr tag is used for generating the docs on XML Attributes.

@attr name name -- declares an xml attribute. The comment here can come from an @attr description tag. name should be what you want the developer to see. (In android, this tag is added by aapt to the R files.)
@attr ref field -- references a field that has an @attr name tag on it.
@attr description more_tags -- defines the docs that are pulled into the XML Attributes section.
prettyprint

Although not a tag, Doclava allows you to print a block of formatted code as follows: `/** * An example code snippet: * <pre class="prettyprint"> * public class MyClass { * public void myMethod() {} * } * </pre> */`

### Tags that are not supported

@author, @version, @serial

We just haven't gotten to this yet.

@since

Again, we just haven't gotten there, but versioning is also supported across the entire api through versioned xml files. See CommandLineArguments.

## Using Doclava with Ant

Ant's built-in javadoc task can be configured to use third-party doclets. The following is an example of how Doclava builds its own documentation:

```
<project> ...
  <target name="doclava" depends="jar">
    <javadoc packagenames="com.google.*" destdir="build/docs" sourcepath="src" docletpath="${jar.file}" bootclasspath="${javahome}/jre/lib/rt.jar" >
      <doclet name="com.google.doclava.Doclava">
        <param name="-stubs" value="build/stubs" />
        <param name="-hdf"/> <param name="project.name"/> <param name="Doclava"/> <!-- versioning -->
        <param name="-since"/> <param name="doclava/v1.txt"/> <param name="v1" />
        <param name="-api"/> <param name="doclava/v2.txt"/>
        <!-- federation -->
        <param name="-federate" /><param name="JDK"/> <param name="http://download.oracle.com/javase/6/docs/api/index.html?"/>
        <param name="-federationapi"/><param name="JDK"/> <param name="http://doclava.googlecode.com/svn/static/api/openjdk-6.xml"/>
      </doclet>
    </javadoc>
  </target>
</project>
```

Note that some command line arguments take more than one value, and so cannot be specified using <param name="-foo" value="bar"/> elements. Instead, list each value in its own element, as in <param name="-foo"/> <param name="val1"/> <param name="val2"/>.

## Using Doclava with Maven

See Maven's guide for using alternate doclets. Use the doclet "com.google.doclava.Doclava", and you can refer to Doclava via our deployed artifact in Maven's central repository. An example of using Doclava with Maven:

```
<project> ...
  <build>
    <plugins> ...
      <plugin>
        <groupId>org.apache.maven.plugins</groupId>
        <artifactId>maven-javadoc-plugin</artifactId>
        <version>2.7</version>
        <configuration>
          <docletArtifact>
            <groupId>com.google.doclava</groupId>
            <artifactId>doclava</artifactId>
            <version>1.0.5</version>
          </docletArtifact>
          <doclet>com.google.doclava.Doclava</doclet>
          <!-- | bootclasspath required by Sun's JVM --> <bootclasspath>${sun.boot.class.path}</bootclasspath>
          <additionalparam> -quiet -federate JDK http://download.oracle.com/javase/6/docs/api/index.html? -federationapi JDK http://doclava.googlecode.com/svn/static/api/openjdk-6.xml -hdf project.name "${project.name}" -d ${project.build.directory}/apidocs </additionalparam>
          <useStandardDocletOptions>false</useStandardDocletOptions>
          <!-- | Apple's JVM sometimes requires more memory -->
          <additionalJOption>-J-Xmx1024m</additionalJOption>
        </configuration>
      </plugin>
    </plugins>
  </build>
</project>
```

The use of bootclasspath parameter is strongly recommended if users compile their javadoc with Sun's JVM, it's not required on Apple's JVM that ignores it. If you intend the API docs be generated across multiple JVMs, we suggest to put it as shown above.

You can build your documentation with the mvn javadoc:javadoc or mvn site goal.

## Simple Customizations

### Add a title

You can set a title for your project by passing an HDF variable as an argument to doclava. Add the argument:

-hdf project.name "Your Project's Name"

### Add an overview page

See Narrative Documentation.

### Change the style

By default, Doclava includes an empty "assets/customizations.css" file as the last included css file. By overriding this file, you can modify the look and feel of your page.

To include your own customizations, add an argument:

`-templatedir my_template_dir`

By specifying this directory, you can override any of the built-in assets and templates . An example custom template directory might have the following structure:

```
my_template_dir/
  assets/
    customizations.css
    customizations.js
  components/
    api_filter.cs
    left_nav.cs
    masthead.cs
    search_box.cs
  customizations.cs
  footer.cs
```

## Federated Documentation

Doclava supports federated documentation. If your project depends on another project that was generated by Doclava or has publicly available source code, Doclava can automatically create links to classes and methods in the remote site for that other project.

Federation requires two things of the other project: an available documentation site, and a machine-readable description of the site's contents. Doclava uses a .txt file for this description.

For documentation sites built with Doclava, this file is automatically placed in the generated documentation, and so you can configure federation with the arguments:

`-federate Project http://project.com/reference`

For projects with documentation you want to link to that have no .txt file, you can generate one for that codebase by running Doclava on its source code with arguments:

`-nodocs -api path/where/to/write/api.txt`

and then federate against that project's existing documentation with:

`-federate Project http://project.com/reference -federationxml Project path/to/api.txt`

The federation xml file may be a local file or an http link. We have several [Android and Open JDK sample api.xml](http://doclava.googlecode.com/svn/static/api/) files available for use.

## Changelog

Changes in this version relative to http://doclava.googlecode.com/
------------------------------------------------------------------
* Added a new command line option -showAnnotation <@interface classname>,
  which takes in a fully qualified annotation classname.  The specified
  annotation will override any @hide annotations within the javadoc.  To
  specify multiple annotations to override @hide, use multiple
  -showAnnotation options.
* Modified the Java stub generator code to write out annotations for
  methods and fields as well, not just classes. This meant adding a
  writeAnnotations call to writeMethod and to writeField
* Modified the writeAnnotations method to take a "isDeprecated"
  parameter, which when set adds a @Deprecated annotation if one
  doesn't already exist. This ensures that APIs only marked with a
  @deprecated documentation comment will also be marked as
  @Deprecated. Also strips out @Override annotations.

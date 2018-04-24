<?cs def:custom_masthead() ?>
<div id="header">
    <div id="headerLeft">
    <?cs if:project.product_line ?>
    <a href="http://www.ooyala.com">
    <img src="http://www.ooyala.com/sites/all/themes/ooyala/pub/images/ooyala-logo-light.svg" width="120px"/></a>
      <span id="masthead-title">
        <a href="http://help.ooyala.com/">Help Center</a> &gt; <a href="http://apidocs.ooyala.com/">API Docs</a> &gt; <?cs var:project.product_line ?> &gt; <?cs var:project.sdk_name ?></span>
    <?cs /if ?>
    </div>
    <div id="headerRight">
        <?cs call:default_search_box() ?>
        <?cs if:reference && reference.apilevels ?>
          <?cs call:default_api_filter() ?>
        <?cs /if ?>
    </div><!-- headerRight -->
</div><!-- header -->
<?cs /def ?>

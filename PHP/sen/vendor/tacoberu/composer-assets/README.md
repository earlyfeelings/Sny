Composer Assets
===============

Copies files from vendor to www directory. The list of files is stored in a json file.

Installation
------------

The recommended way to install is via Composer:

        composer require tacoberu/composer-assets



Usage
-----

```json
    {
    	"require": {
    		"tacoberu/composer-assets": "*",
    	},
        "scripts": {
            "post-autoload-dump": [
                "Taco\\ComposerScripts\\CopyAssetsToPublic::process"
            ]
        },
        "config": {
    		"www-dir": "public",
    		"assets-definition": "scripts/assets.json"
    	}
    }
```

Add list of files definition (vendor-src -> public-desc) in scripts/assets.json:
```json
{
	"www:/assets/js/netteForms.js": [
		"vendor:/nette/forms/src/assets/netteForms.js"
	],
	"www:/assets/img/logo.png": [
		"vendor:/nette/forms/examples/assets/logo.png"
	],
	"www:/assets/style.css": [
		"../app/examples/assets/style-1.css",
		"../app/examples/assets/style-2.css",
		"../app/examples/assets/style-3.css"
	]
}
```

And run:

        composer install

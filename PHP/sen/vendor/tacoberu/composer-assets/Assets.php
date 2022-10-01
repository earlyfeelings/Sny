<?php
/**
 * Copyright (c) since 2004 Martin Takáč
 * @author Martin Takáč <martin@takac.name>
 */

namespace Taco\ComposerScripts;

use Composer\Script\Event;
use LogicException;


/**
 * Copies files from vendor to www directory. The list of files is stored in a json file.
 * To composer.config append option 'www-dir' for location of www directory.
 * To composer.config append option 'assets-definition' for location of list of assets.
 */
class CopyAssetsToPublic
{
	static function process(Event $ev)
	{
		$vendorDir = $ev->getComposer()->getConfig()->get('vendor-dir');
		$wwwDir = self::requireWwwDir($ev->getComposer()->getConfig()->get('www-dir'));
		$definitionFile = self::requireLocation($ev->getComposer()->getConfig()->get('assets-definition'));

		// seskupení
		$tree = json_decode(file_get_contents($definitionFile));

		// zápis podle skupin
		foreach ($tree as $desc => $items) {
			self::assertStartWithWWW($desc);
			$ev->getIO()->write("\tcopy-to: '$desc'");
			$content = [];
			foreach ($items as $x) {
				$content[] = file_get_contents(self::resolvePath($x, $definitionFile, $vendorDir, $wwwDir));
			}
			$desc = ltrim(substr($desc, 4), '/');
			file_put_contents(self::requireDir($wwwDir, dirname($desc)) . '/' . basename($desc), implode("\n\n\n", $content));
		}

		$ev->getIO()->write("\tdone");
	}



	private static function assertStartWithWWW($path)
	{
		if (strncmp($path, 'www:', 4) !== 0) {
			throw new LogicException("Destination path '$path' must start 'www:' prefix.");
		}
	}



	private static function resolvePath($x, $definitionFile, $vendorDir, $wwwDir)
	{
		switch (True) {
			case strncmp($x, 'vendor:', 7) === 0:
				return self::buildPath($vendorDir, substr($x, 7));
			case strncmp($x, 'www:', 4) === 0:
				return self::buildPath($wwwDir, substr($x, 4));
			default:
				return self::buildPath(dirname($definitionFile), $x);
		}
	}



	private static function buildPath($prefix, $path)
	{
		return $prefix . '/' . ltrim($path, '/');
	}



	private static function requireWwwDir($path)
	{
		if (file_exists($path)) {
			return realpath($path);
		}

		$path2 = realpath(__dir__ . '/../' . $path);
		if (file_exists($path2)) {
			return $path2;
		}

		throw new LogicException("Unknow path '$path'.");
	}



	private static function requireLocation($loc)
	{
		if (file_exists($loc)) {
			return realpath($loc);
		}
		throw new LogicException("Unknow location '$loc'.");
	}



	private static function requireDir($base, $path)
	{
		$paths = explode('/', $path);
		$ret = $base;
		foreach ($paths as $x) {
			$ret .= '/' . $x;
			if ( ! file_exists($ret)) {
				mkdir($ret);
			}
		}
		return $ret;
	}

}

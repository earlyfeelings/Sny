<?php
declare(strict_types=1);

namespace App\Factory\Forms;

use Nette\Application\UI\Form;


class FormFactory
{

	function create() : Form {
		$form = new Form();
		$form->addProtection();
		return $form;
	}

}

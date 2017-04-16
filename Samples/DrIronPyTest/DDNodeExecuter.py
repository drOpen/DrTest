import clr
import os

DD_NODE_ATTR_NAME = {'module_name': 'Module', 'class_name': 'ClassName', 'method_name': 'MethodName'}


def concatenate_win_path(root, *args):
    path = root
    for f in args:
        path = os.path.join(path, f)
    return os.path.abspath(path)


def add_references_from_path(path, libs_list):
    """
    Adding references to c# libs.
    :param path: name of subfolder with libs (%project_folder%\path)
    :param libs_list: a list of libs that we want to add to references
    :return: none
    """
    lib_path = concatenate_win_path(os.path.dirname(__file__), path)  # here we get windows path to folder with libs
    for lib in libs_list:
        clr.AddReferenceToFileAndPath(concatenate_win_path(lib_path, lib))  # and here we adding all needed c# libs


def read_dd_node_from_file(path):
    # TBD
    return execute_static_method_dynamically('DrAction.FSAction', 'DDTest', 'LoadDDNodeAsFile', path)


def get_module_instance(module_name):
    """
    Importing module of class and return an instance of module
    :param module_name: Module in format 'AAA.BBB.CCC'
    :return: Instance of module
    """
    module_list = module_name.split('.')
    result_instance = __import__(module_list[0])
    for module in module_list[1:]:
        result_instance = getattr(result_instance, module)
    return result_instance


def execute_static_method_dynamically(module_name, class_name, method_name, *args):
    """
    Execute static method dynamically.
    :param module_name: Name of the module from which some method will be executed
    :param class_name: Name of the class in the module module_name from which some method will be executed
    :param method_name: Name of the method in class class_name and module module_name that will be executed
    :param args: Arguments of method method_name
    :return: Return a return value of method method_name
    """
    module_ins, class_ins, result = None, None, None
    try:
        module_ins = get_module_instance(module_name)  # importing modules and getting module instance
        class_ins = getattr(module_ins, class_name)  # getting class instance
        method_ins = getattr(class_ins, method_name)  # getting method instance
        result = method_ins(*args)  # executing method
    except ImportError:
        print('Module {module_name} not found'.format(module_name=module_name))
        raise  # TBD
    except AttributeError:
        if class_ins is None:
            print('Class {class_name}({module_name}) not found'.format(class_name=class_name, module_name=module_name))
            raise
        else:
            print('Method {method_name} of class {class_name}({module_name}) not found'.format
                  (method_name=method_name, class_name=class_name, module_name=module_name))
            raise  # TBD
    return result


def execute_method_dynamically(module_name, class_name, method_name, constructor_params, instance, *args):
    """
    Execute non-static method dynamically.
    :param module_name: Name of the module from which some method will be executed
    :param class_name: Name of the class in the module module_name from which some method will be executed
    :param method_name: Name of the method in class class_name and module module_name that will be executed
    :param constructor_params: The list of parameters for constructor of creating class.
    :param instance: An instance in which the method will be called
    :param args: Arguments of method method_name
    :return: Resulting DDNode with time of execution, status and, possibly, exceptions and the instance of class.
    """
    module_ins, class_ins, result = None, None, None
    try:
        if instance is None:  # If we still do not have an instance of class...
            module_ins = get_module_instance(module_name)  # importing modules and getting a module instance
            class_ins = getattr(module_ins, class_name)  # getting a class instance
            obj = class_ins(*constructor_params)  # calling a constructor of a class
        else:
            obj = instance  # using already existing instance
        method_ins = getattr(obj, method_name)  # getting method instance
        result = method_ins(*args)  # executing method
    except ImportError:
        print('Module {module_name} not found'.format(module_name=module_name))
        raise  # TBD
    except AttributeError:
        if class_ins is None:
            print('Class {class_name}({module_name}) not found'.format(class_name=class_name, module_name=module_name))
            raise #TBD
        else:
            print('Method {method_name} of class {class_name}({module_name}) not found'.format
                  (method_name=method_name, class_name=class_name, module_name=module_name))
            raise  # TBD
    return result, obj


def dd_run_static_method(dd_node):
    """
    Running of static method from c# lib dynamically.
    :param dd_node: DDNode with names of Module, Class, Method and params for this method
    :return: Resulting DDNode with time of execution, status and, possibly, exceptions.
    """
    return execute_static_method_dynamically(dd_node.Attributes[DD_NODE_ATTR_NAME['module_name']].GetValueAsString(),
                                             dd_node.Attributes[DD_NODE_ATTR_NAME['class_name']].GetValueAsString(),
                                             dd_node.Attributes[DD_NODE_ATTR_NAME['method_name']].GetValueAsString(),
                                             dd_node)


def dd_run_non_static_method(dd_node, constructor_params=None, instance=None):
    """
    Running of non-static method from c# method.
    There are two ways to use this method:
    1) Creating of instance of Class from DDNode and calling of method of this object.
    Constructor of parameter will be call with parameters from parameter constructor_params.
    2) Calling of method of an existing object. In this case 'instance' parameter should be not 'None' and value of
    'constructor_value' will be ignored.
    :param dd_node: DDNode with names of Module, Class, Method and params for this method
    :param constructor_params: The list of parameters for constructor of creating class.
    :param instance: An instance in which the method will be called
    :return: Resulting DDNode with time of execution, status and, possibly, exceptions and the instance of class.
    """
    if constructor_params is None:
        constructor_params = []
    result_dd_node, obj = execute_method_dynamically\
        (dd_node.Attributes[DD_NODE_ATTR_NAME['module_name']].GetValueAsString(),
         dd_node.Attributes[DD_NODE_ATTR_NAME['class_name']].GetValueAsString(),
         dd_node.Attributes[DD_NODE_ATTR_NAME['method_name']].GetValueAsString(),
         constructor_params, instance, dd_node)
    return result_dd_node, obj
